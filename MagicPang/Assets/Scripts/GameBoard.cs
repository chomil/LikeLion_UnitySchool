using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

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

    public void CheckMatch()
    {
    }


    public void SwapTile(Tile tile1, Tile tile2)
    {
        if (tile1 == null || tile2 == null)
        {
            return;
        }

        (tile1.tileIndex, tile2.tileIndex) = (tile2.tileIndex, tile1.tileIndex);
        (tiles[tile1.tileIndex], tiles[tile2.tileIndex]) = (tiles[tile2.tileIndex], tiles[tile1.tileIndex]);
        LinkingTile(tile1);
        LinkingTile(tile2);
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
}