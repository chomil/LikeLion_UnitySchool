using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
    public List<Tile> roads = new List<Tile>();

    public Character spawnCharacter = null;
    public Character selectCharacter = null;


    public void BuyCharacter(int chacterIndex)
    {
        spawnCharacter = Instantiate(GameManager.inst.characterPrefabs[chacterIndex]);
        spawnCharacter.transform.forward = Vector3.back;
        spawnCharacter.SetTemp(true);
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