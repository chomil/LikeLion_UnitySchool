using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feel;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int tilePos;
    public bool isRoad = false;
    public bool isStart = false;

    void Start()
    {
        tilePos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        GameManager.inst.curStage.tiles.Add(tilePos,this);
    }
}
