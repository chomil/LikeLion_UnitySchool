using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shapes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public List<Tile> roads = new List<Tile>();

    public List<Monster> monsters = new List<Monster>();
    public List<Character> characters = new List<Character>();

    public Character spawnCharacter = null;
    public Character selectCharacter = null;

    public GameObject shop;

    public Rectangle beatSquare;
    public GameObject startButton;

    public int stageLevel = 1;
    public int diedMonsterNum = 0;
    public bool isPlaying = false;


    public void StartStage()
    {
        Debug.Log("Start");
        StartCoroutine(PlayStageCoroutine());
    }

    public IEnumerator PlayStageCoroutine()
    {
        shop.SetActive(false);
        startButton.SetActive(false);
        isPlaying = true;
        diedMonsterNum = 0;
        
        for (int i = 0; i <= 10; i++)
        {
            yield return null;
            beatSquare.Width = Mathf.Lerp(beatSquare.Width, 100f, i / 10f);
        }
        
        SoundManager.inst.PlayBGM(GameManager.inst.stageBgm,0.2f);
        SoundManager.inst.bgmBpm = 105f;

        monsters.Clear();
        for (int i = 0; i < GetMonNumInLevel(); i++)
        {
            Monster curMon = Instantiate(GameManager.inst.monsterPrefabs[0],roads[0].transform.position,quaternion.identity);
            curMon.spawnIndex = i;
            monsters.Add(curMon);
        }
    }

    public int GetMonNumInLevel()
    {
        return 3 + 2 * stageLevel;
    }
    public IEnumerator EndStageCoroutine()
    {
        isPlaying = false;
        shop.SetActive(true);
        startButton.SetActive(true);
        
        for (int i = 0; i <= 10; i++)
        {
            yield return null;
            beatSquare.Width = Mathf.Lerp(beatSquare.Width, 300f, i / 10f);
        }
        
        SoundManager.inst.PlayBGM(GameManager.inst.defaultBgm,0.2f);
        SoundManager.inst.bgmBpm = 84f;
    }

    public void SpawnMonster(int monsterIndex)
    {
        Instantiate(GameManager.inst.monsterPrefabs[monsterIndex],roads[0].transform.position,quaternion.identity);
    }

    public void BuyCharacter(int chacterIndex)
    {
        if (selectCharacter)
        {
            SetSelectCharacter(selectCharacter);
        }

        foreach (Character character in characters)
        {
            character.SetInteractive(false);
        }
        
        spawnCharacter = Instantiate(GameManager.inst.characterPrefabs[chacterIndex]);
        spawnCharacter.characterMesh.transform.forward = Vector3.back;
        SetSelectCharacter(spawnCharacter);
        spawnCharacter.SetInteractive(false);
        
        shop.SetActive(false);
    }
    
    

    public void SetSelectCharacter(Character character)
    {
        if (selectCharacter == character)
        {
            selectCharacter.SetSelect(false);
            selectCharacter = null;
            return;
        }
        if (selectCharacter)
        {
            selectCharacter.SetSelect(false);
        }
        selectCharacter = character;
        selectCharacter.SetSelect(true);
    }
    
    private void Start()
    {
        GameManager.inst.curStage = this;
        StartCoroutine(InitializeRoads());
    }

    private void Update()
    {
        if (isPlaying)
        {
            if (diedMonsterNum == GetMonNumInLevel())
            {
                StartCoroutine(EndStageCoroutine());
            }
        }
    }

    IEnumerator InitializeRoads()
    {
        yield return null;

        Tile startRoad = null;
        foreach (var tilePair in tiles)
        {
            if (tilePair.Value.isStart == true)
            {
                startRoad = tilePair.Value;
                break;
            }
        }

        if (startRoad)
        {
            roads.Add(startRoad);

            Tile curRoad = startRoad;
            Vector2Int[] dir =
                { new Vector2Int(0, 2), new Vector2Int(0, -2), new Vector2Int(-2, 0), new Vector2Int(2, 0) };
            bool isEnd = false;
            while (isEnd == false)
            {
                Vector2Int curPos = curRoad.tilePos;
                for (int i = 0; i < 4; i++)
                {
                    if (tiles.TryGetValue(curPos + dir[i], out Tile curTile))
                    {
                        if (curTile.isRoad && (roads.Count == 1 || roads[^2] != curTile))
                        {
                            roads.Add(curTile);
                            curRoad = curTile;
                            break;
                        }
                    }

                    if (i == 3)
                    {
                        isEnd = true;
                    }
                }
            }
        }
    }
}