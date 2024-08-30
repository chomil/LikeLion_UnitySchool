using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Shapes;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    public GameObject noteGround;
    public List<RhythmNote> notes = new List<RhythmNote>();
    public RhythmSquare rhythmSquare;


    public void StartStage()
    {
        Debug.Log("Start");
        StartCoroutine(PlayStageCoroutine());
    }

    public IEnumerator PlayStageCoroutine()
    {
        SetSelectCharacter(selectCharacter);

        shop.SetActive(false);
        startButton.SetActive(false);
        isPlaying = true;
        diedMonsterNum = 0;


        for (int i = 0; i <= 10; i++)
        {
            yield return null;
            beatSquare.Width = Mathf.Lerp(beatSquare.Width, 100f, i / 10f);
        }

        SoundManager.inst.PlayBGM(GameManager.inst.stageBgm, 0.2f);
        SoundManager.inst.bgmBpm = 105f;


        foreach (RhythmNote curNote in notes)
        {
            curNote.ResetNote();
        }

        Dictionary<CharacterType, List<Character>> typeCount = new Dictionary<CharacterType, List<Character>>();
        typeCount.Add(CharacterType.Sword, new List<Character>());
        typeCount.Add(CharacterType.Bow, new List<Character>());
        typeCount.Add(CharacterType.Magic, new List<Character>());
        foreach (Character curCharacter in characters)
        {
            curCharacter.SetInteractive(false);
            typeCount[curCharacter.characterType].Add(curCharacter);
        }

        //0,1/2,3/4,5/6,7
        if (typeCount[CharacterType.Sword].Count > 0)
        {
            int randomIndex = Random.Range(6, 8);
            foreach (Character curCharacter in typeCount[CharacterType.Sword])
            {
                notes[randomIndex].linkedCharacter.Add(curCharacter);
            }

            if (notes[randomIndex].type == KeyType.None)
            {
                notes[randomIndex].SetNote(KeyType.Down);
            }
        }

        if (typeCount[CharacterType.Bow].Count > 0)
        {
            int randomIndex = Random.Range(2, 4);
            foreach (Character curCharacter in typeCount[CharacterType.Bow])
            {
                notes[randomIndex].linkedCharacter.Add(curCharacter);
            }

            if (notes[randomIndex].type == KeyType.None)
            {
                notes[randomIndex].SetNote(KeyType.Drag, KeyCode.None, 1);
                //notes[randomIndex + 2].SetLongNote(true, 1);
            }
        }

        if (typeCount[CharacterType.Magic].Count > 0)
        {
            int randomIndex = Random.Range(0, 2);
            foreach (Character curCharacter in typeCount[CharacterType.Magic])
            {
                notes[randomIndex].linkedCharacter.Add(curCharacter);
            }

            if (notes[randomIndex].type == KeyType.None)
            {
                notes[randomIndex].SetNote(KeyType.Drag, KeyCode.None, 2);
                //notes[randomIndex + 4].SetLongNote(true, 2);
            }
        }

        monsters.Clear();
        for (int i = 0;
             i < GetMonNumInLevel();
             i++)
        {
            Monster curMon = Instantiate(GameManager.inst.monsterPrefabs[0], roads[0].transform.position,
                quaternion.identity);
            curMon.spawnIndex = i;
            monsters.Add(curMon);
        }

        yield return new WaitForSeconds(1f);
        noteGround.GetComponent<RectTransform>().DOSizeDelta(new Vector2(1500f, 120f), 1f);
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
        stageLevel++;
        noteGround.GetComponent<RectTransform>().DOSizeDelta(new Vector2(0f, 120f), 0.5f);

        foreach (Character curCharacter in characters)
        {
            curCharacter.SetInteractive(true);
        }

        for (int i = 0; i <= 10; i++)
        {
            yield return null;
            beatSquare.Width = Mathf.Lerp(beatSquare.Width, 300f, i / 10f);
        }

        SoundManager.inst.PlayBGM(GameManager.inst.defaultBgm, 0.2f);
        SoundManager.inst.bgmBpm = 84f;
    }

    public void SpawnMonster(int monsterIndex)
    {
        Instantiate(GameManager.inst.monsterPrefabs[monsterIndex], roads[0].transform.position, quaternion.identity);
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
        if (selectCharacter)
        {
            selectCharacter.SetSelect(false);
            if (selectCharacter == character)
            {
                selectCharacter = null;
                return;
            }
        }

        selectCharacter = character;
        if (selectCharacter)
        {
            selectCharacter.SetSelect(true);
        }
    }

    private void Start()
    {
        GameManager.inst.curStage = this;
        StartCoroutine(InitializeRoads());
        InitializeNotes();
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


    public void InitializeNotes()
    {
        notes.Clear();
        for (int i = 0; i < 4; i++)
        {
            RhythmNote curNote = Instantiate(GameManager.inst.notePrefab, noteGround.transform);
            curNote.InitNote(i, Vector3.right);
            notes.Add(curNote);

            curNote = Instantiate(GameManager.inst.notePrefab, noteGround.transform);
            curNote.InitNote(i, Vector3.left);
            notes.Add(curNote);
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