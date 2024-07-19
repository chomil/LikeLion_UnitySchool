using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{
    private const int TileCntX = 8;
    private const int TileCntY = 8;
    
    void Start()
    {
        GameManager.inst.curBoard = this;


        for (int x = 0; x < TileCntX; x++)
        {
            for (int y = 0; y < TileCntY; y++)
            {
                int spawnPosX = 60 + 120 * x;
                int spawnPosY = -60 - 120 * y;
                int randomTileColor = Random.Range(0, (int)Tile_Color.None);

                Tile newTile = Instantiate(GameManager.inst.tilePrefabs[randomTileColor], transform);
                newTile.transform.localPosition = new Vector3(spawnPosX, spawnPosY, 0);

            }
        }
    }

    void Update()
    {
    }
}