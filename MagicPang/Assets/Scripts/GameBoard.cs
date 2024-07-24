using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using TMPro;

public class GameBoard : MonoBehaviour
{
    private GameData gameData;

    public TextMeshProUGUI coinText;
    
    private const int TileCntX = 8;
    private const int TileCntY = 8;
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    public bool isMoving = false;

    public Player curPlayer = null;
    public Monster curMonster = null;
    public BackgroundMover curBackGround = null;

    public Skill skill = Skill.None;


    void Start()
    {
        gameData = GameManager.inst.gameData;
        
        GameManager.inst.curBoard = this;

        
        for (int y = 0; y < TileCntY; y++)
        {
            for (int x = 0; x < TileCntX; x++)
            {
                Vector2Int spawnIndex = new Vector2Int(x, y);
                Elemental randomTileElemental;
                bool horizontalMatch;
                bool verticalMatch;
                do
                {  
                    randomTileElemental = (Elemental)Random.Range(0, (int)Elemental.None);
                    horizontalMatch = false;
                    verticalMatch = false;
                    if (x >= 2)
                    {
                        horizontalMatch = tiles[new Vector2Int(x - 1, y)].elemental ==
                                          tiles[new Vector2Int(x - 2, y)].elemental &&
                                          tiles[new Vector2Int(x - 1, y)].elemental == randomTileElemental;
                    }
                    if (y >= 2)
                    {
                        verticalMatch = tiles[new Vector2Int(x, y - 1)].elemental ==
                                        tiles[new Vector2Int(x, y - 2)].elemental &&
                                        tiles[new Vector2Int(x, y - 1)].elemental == randomTileElemental;
                    }
                } while (horizontalMatch||verticalMatch);
                
                
                Tile newTile = Instantiate(GameManager.inst.tilePrefabs[(int)randomTileElemental], transform);
                newTile.transform.localPosition = TileIndexToLocalPos(spawnIndex);
                newTile.tileIndex = spawnIndex;
                tiles.Add(newTile.tileIndex, newTile);

                LinkingTile(newTile);
            }
        }
        
        
        AddCoin(0);
        SpawnNewMonster(gameData.monsterIndex);
        curMonster.transform.localPosition = new Vector3(1.2f, curMonster.transform.position.y, curMonster.transform.position.z);
        GameManager.inst.SaveGameData();
    }

    void Update()
    {
        if (isMoving)
        {
            return;
        }
        
        
    }

    public bool CheckMatchAll()
    {
        bool isMatch = false;

        for (int y = 0; y < TileCntY; y++)
        {
            for (int x = 0; x < TileCntX; x++)
            {
                Vector2Int tileIndex = new Vector2Int(x, y);
                if (tiles.TryGetValue(tileIndex, out var curTile) == false)
                {
                    continue;
                }

                if (curTile.isMatched)
                {
                    isMatch = true;
                }

                if (curTile && curTile.nearTiles[1] && curTile.nearTiles[1].nearTiles[1]) //vertical match search
                {
                    if (curTile.elemental == curTile.nearTiles[1].elemental &&
                        curTile.elemental == curTile.nearTiles[1].nearTiles[1].elemental)
                    {
                        curTile.isMatched = true;
                        curTile.nearTiles[1].isMatched = true;
                        curTile.nearTiles[1].nearTiles[1].isMatched = true;

                        isMatch = true;
                    }
                }

                if (curTile && curTile.nearTiles[3] && curTile.nearTiles[3].nearTiles[3]) //horizontal match search
                {
                    if (curTile.elemental == curTile.nearTiles[3].elemental &&
                        curTile.elemental == curTile.nearTiles[3].nearTiles[3].elemental)
                    {
                        curTile.isMatched = true;
                        curTile.nearTiles[3].isMatched = true;
                        curTile.nearTiles[3].nearTiles[3].isMatched = true;

                        isMatch = true;
                    }
                }
            }
        }

        return isMatch;
    }

    public IEnumerator Match()
    {
        bool isMatch = CheckMatchAll();

        if (isMatch)
        {
            isMoving = true;
            yield return StartCoroutine(PoppingTiles());
            yield return StartCoroutine(FallingTiles());
            StartCoroutine(Match());
        }
        else
        {
            curPlayer.AttackEnd();
            yield return StartCoroutine(MonsterTurn());
            isMoving = false;
            Debug.Log("My Turn");
        }
    }


    public void SpawnNewMonster(int monsterIndex = -1)
    {
        //Spawn new monster
        if (monsterIndex == -1)
        {
            monsterIndex =  Random.Range(0, 10);
        }
        Monster spawnMon = GameManager.inst.monsterPrefabs[monsterIndex];
        Vector3 spawnPos = new Vector3(5f, 2.35f, 1.8f);
        curMonster = Instantiate(spawnMon, spawnPos, quaternion.identity);
        curMonster.level = gameData.stageLevel;
            
        gameData.monsterIndex = monsterIndex;
    }
    
    public IEnumerator MonsterTurn()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("monster Turn");
        if (curMonster.hp == 0)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(curMonster.dieEffect, curMonster.hitPos.transform.position, curMonster.transform.rotation);
            AddCoin(curMonster.level*10);
            Destroy(curMonster.gameObject);
            
            //Next Monster
            gameData.stageLevel++;
            SpawnNewMonster();
            
            //Save Data
            gameData.maxHp = curPlayer.maxHp;
            gameData.hp = curPlayer.hp;
            GameManager.inst.SaveGameData();
            
            yield return curMonster.StartCoroutine(curMonster.MoveCoroutine());
        }
        else
        {
            yield return curMonster.StartCoroutine(curMonster.AttackCoroutine());
        }
        yield return null;
    }

    public void AddCoin(int coinNum)
    {
        gameData.coin += coinNum;
        StartCoroutine(AddCoinCoroutine());
    }

    private IEnumerator AddCoinCoroutine()
    {
        int startCoin = int.Parse(coinText.text);
        for (int i = 0; i <= 10; i++)
        {
            int lerfCoin = (int)Mathf.Lerp(startCoin, gameData.coin, 0.1f * i);
            coinText.text = lerfCoin.ToString();
            yield return new WaitForSeconds(0.03f);
        }
        coinText.text = gameData.coin.ToString();
    }

    public void DeleteTile(Vector2Int tileIndex)
    {
        if (tiles.TryGetValue(tileIndex, out var curTile))
        {
            DeleteTile(curTile);
        }
    }

    public void DeleteTile(Tile tile)
    {
        if (tile)
        {
            tiles[tile.tileIndex] = null;
            Destroy(tile.gameObject);
        }
    }

    public IEnumerator PoppingTiles()
    {
        Debug.Log("PoppingTiles");

        List<Tile> poppingTiles = new List<Tile>();

        for (int y = 0; y < TileCntY; y++)
        {
            for (int x = 0; x < TileCntX; x++)
            {
                Vector2Int tileIndex = new Vector2Int(x, y);
                if (tiles[tileIndex].isMatched == true)
                {
                    poppingTiles.Add(tiles[tileIndex]);
                }
            }
        }

        Vector3 newScale = new Vector3(1.25f, 1.25f, 1);
        for (int i = 0; i < poppingTiles.Count; i++)
        {
            curPlayer.AddAttack(poppingTiles[i].elemental);
            poppingTiles[i].transform.DOScale(newScale, 0.15f).SetLoops(2, LoopType.Yoyo);
        }

        yield return new WaitForSeconds(0.4f);


        for (int i = 0; i < poppingTiles.Count; i++)
        {
            DeleteTile(poppingTiles[i]);
        }
        
        
        yield return null;
    }

    public IEnumerator FallingTiles()
    {
        Debug.Log("FallingTiles");

        List<Tile> fallingTiles = new List<Tile>();

        for (int x = 0; x < TileCntX; x++)
        {
            int lastY = TileCntY - 1;
            for (int y = TileCntY - 1; y >= 0; y--) //current tile fall
            {
                Vector2Int tileIndex = new Vector2Int(x, y);

                if (tiles.TryGetValue(tileIndex, out var curTile) == false)
                {
                    continue;
                }

                if (curTile && curTile.isMatched == false) //After falling
                {
                    Vector2Int prevIndex = curTile.tileIndex;
                    Vector2Int targetIndex = new Vector2Int(x, lastY);
                    lastY--;

                    if (curTile.tileIndex == targetIndex)
                    {
                        continue;
                    }

                    fallingTiles.Add(curTile);
                    curTile.transform.DOLocalMove(TileIndexToLocalPos(targetIndex), 0.5f).SetEase(Ease.InOutCubic)
                        .OnComplete(() =>
                        {
                            curTile.tileIndex = targetIndex;
                            tiles[curTile.tileIndex] = curTile;
                        });
                }
            }

            for (int y = lastY; y >= 0; y--) //new tile spawn
            {
                int spawnNum = lastY - y + 1;
                Vector3 spawnPos = TileIndexToLocalPos(new Vector2Int(x, -spawnNum));

                int randomTileColor = Random.Range(0, (int)Elemental.None);
                Tile newTile = Instantiate(GameManager.inst.tilePrefabs[randomTileColor], transform);
                newTile.transform.localPosition = spawnPos;


                Vector2Int targetIndex = new Vector2Int(x, y);

                fallingTiles.Add(newTile);
                newTile.transform.DOLocalMove(TileIndexToLocalPos(targetIndex), 0.5f).SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                    {
                        newTile.tileIndex = targetIndex;
                        tiles[newTile.tileIndex] = newTile;
                    });
            }
        }

        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i < fallingTiles.Count; i++)
        {
            LinkingTile(fallingTiles[i]);
        }
        
        yield return null;
    }


    public void SwapTile(Tile tile1, Tile tile2, bool isBack = false)
    {
        if (tile1 == null || tile2 == null)
        {
            return;
        }

        isMoving = true;

        tile1.transform.DOLocalMove(TileIndexToLocalPos(tile2.tileIndex), 0.5f);
        tile2.transform.DOLocalMove(TileIndexToLocalPos(tile1.tileIndex), 0.5f).OnComplete(
            () =>
            {
                (tile1.tileIndex, tile2.tileIndex) = (tile2.tileIndex, tile1.tileIndex);
                tiles[tile1.tileIndex] = tile1;
                tiles[tile2.tileIndex] = tile2;

                LinkingTile(tile1);
                LinkingTile(tile2);
                if (CheckMatchAll())
                {
                    StartCoroutine(Match());
                }
                else
                {
                    if (isBack == false)
                    {
                        SwapTile(tile1, tile2, true);
                    }
                    else
                    {
                        isMoving = false;
                    }
                }
            });
    }

    public void UseSkill(Tile clickTile)
    {
        switch (skill)
        {
            case Skill.Punch:
                clickTile.isMatched = true;
                break;
            case Skill.Meteor:
                for (int x = clickTile.tileIndex.x - 1; x <= clickTile.tileIndex.x + 1; x++)
                {
                    for (int y = clickTile.tileIndex.y - 1; y <= clickTile.tileIndex.y + 1; y++)
                    {
                        if (tiles.TryGetValue(new Vector2Int(x, y), out var targetTile))
                        {
                            if (targetTile)
                            {
                                targetTile.isMatched = true;
                            }
                        }
                    }
                }
                break;
            case Skill.Vertical:
                for (int y = 0; y < TileCntY; y++)
                {
                    if (tiles.TryGetValue(new Vector2Int(clickTile.tileIndex.x, y), out var targetTile))
                    {
                        if (targetTile)
                        {
                            targetTile.isMatched = true;
                        }
                    }
                }
                break;
            case Skill.Horizontal:
                for (int x = 0; x < TileCntX; x++)
                {
                    if (tiles.TryGetValue(new Vector2Int(x, clickTile.tileIndex.y), out var targetTile))
                    {
                        if (targetTile)
                        {
                            targetTile.isMatched = true;
                        }
                    }
                }
                break;
        }
        StartCoroutine(Match());
        skill = Skill.None;
    }

    public void LinkingTile(Tile tile)
    {
        if (tile == null)
        {
            return;
        }

        Vector2Int tileIndex = tile.tileIndex;

        if (tiles.TryGetValue(tileIndex + Vector2Int.down, out Tile upTile))
        {
            tile.nearTiles[0] = upTile; //U
            if (upTile)
            {
                upTile.nearTiles[1] = tile;
            }
        }
        else
        {
            tile.nearTiles[0] = null;
        }

        if (tiles.TryGetValue(tileIndex + Vector2Int.up, out Tile downTile))
        {
            tile.nearTiles[1] = downTile; //D
            if (downTile)
            {
                downTile.nearTiles[0] = tile;
            }
        }
        else
        {
            tile.nearTiles[1] = null;
        }

        if (tiles.TryGetValue(tileIndex + Vector2Int.left, out Tile leftTile))
        {
            tile.nearTiles[2] = leftTile; //L
            if (leftTile)
            {
                leftTile.nearTiles[3] = tile;
            }
        }
        else
        {
            tile.nearTiles[2] = null;
        }

        if (tiles.TryGetValue(tileIndex + Vector2Int.right, out Tile rightTile))
        {
            tile.nearTiles[3] = rightTile; //R
            if (rightTile)
            {
                rightTile.nearTiles[2] = tile;
            }
        }
        else
        {
            tile.nearTiles[3] = null;
        }
    }

    public Vector3 TileIndexToLocalPos(Vector2Int tileIndex)
    {
        return new Vector3(60 + 120 * tileIndex.x, -60 - 120 * tileIndex.y);
    }
}