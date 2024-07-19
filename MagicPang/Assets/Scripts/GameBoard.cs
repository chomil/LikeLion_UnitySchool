using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GameBoard : MonoBehaviour
{
    private const int TileCntX = 8;
    private const int TileCntY = 8;
    private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();

    public int movingTile = 0;
    public List<Tile> swapTile = new List<Tile>();


    void Start()
    {
        GameManager.inst.curBoard = this;


        for (int y = 0; y < TileCntY; y++)
        {
            for (int x = 0; x < TileCntX; x++)
            {
                Vector2Int spawnPos = new Vector2Int(60 + 120 * x, -60 - 120 * y);
                int randomTileColor = Random.Range(0, (int)Elemental.None);

                Tile newTile = Instantiate(GameManager.inst.tilePrefabs[randomTileColor], transform);
                newTile.transform.localPosition = new Vector3(spawnPos.x, spawnPos.y, 0);
                newTile.tileIndex = new Vector2Int(x, y);
                tiles.Add(newTile.tileIndex, newTile);

                LinkingTile(newTile);
            }
        }
    }

    void Update()
    {
        if (movingTile > 0)
        {
            return;
        }
    }

    public bool CheckMatch()
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

                if (curTile&& curTile.nearTiles[1]&& curTile.nearTiles[1].nearTiles[1]) //vertical match search
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
                if (curTile&& curTile.nearTiles[3]&& curTile.nearTiles[3].nearTiles[3]) //horizontal match search
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


    public void SwapTile(Tile tile1, Tile tile2)
    {
        if (tile1 == null || tile2 == null)
        {
            return;
        }

        tile1.transform.DOLocalMove(TileIndexToLocalPos(tile2.tileIndex), 0.3f);
        tile2.transform.DOLocalMove(TileIndexToLocalPos(tile1.tileIndex), 0.3f).OnComplete(
            () =>
            {
                (tile1.tileIndex, tile2.tileIndex) = (tile2.tileIndex, tile1.tileIndex);
                (tiles[tile1.tileIndex], tiles[tile2.tileIndex]) = (tiles[tile2.tileIndex], tiles[tile1.tileIndex]);
        
                LinkingTile(tile1);
                LinkingTile(tile2);
                
                
                CheckMatch();
            });
    }

    public void LinkingTile(Tile tile)
    {
        Vector2Int tileIndex = tile.tileIndex;

        if (tiles.TryGetValue(tileIndex + Vector2Int.down, out Tile upTile))
        {
            tile.nearTiles[0] = upTile; //U
            upTile.nearTiles[1] = tile;
        }
        else
        {
            tile.nearTiles[0] = null;
        }

        if (tiles.TryGetValue(tileIndex + Vector2Int.up, out Tile downTile))
        {
            tile.nearTiles[1] = downTile; //D
            downTile.nearTiles[0] = tile;
        }
        else
        {
            tile.nearTiles[1] = null;
        }

        if (tiles.TryGetValue(tileIndex + Vector2Int.left, out Tile leftTile))
        {
            tile.nearTiles[2] = leftTile; //L
            leftTile.nearTiles[3] = tile;
        }
        else
        {
            tile.nearTiles[2] = null;
        }

        if (tiles.TryGetValue(tileIndex + Vector2Int.right, out Tile rightTile))
        {
            tile.nearTiles[3] = rightTile; //R
            rightTile.nearTiles[2] = tile;
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