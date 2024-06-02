using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class AStarBoard : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject tilePrefab;
    public Vector2Int boardSize;
    public Vector2Int startPoint;
    public Vector2Int endPoint;

    Tile[,] tiles;
    Player player;

    List<Tile> tilePath = new List<Tile>();



    void Start()
    {
        tiles = new Tile[boardSize.y, boardSize.x];

        player = Instantiate(playerPrefab, this.transform).GetComponent<Player>();
        player.transform.localPosition = new Vector2(startPoint.x, startPoint.y);

        for (int y = 0; y < boardSize.y; y++)
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                tiles[y, x] = (Instantiate(tilePrefab, this.transform)).GetComponent<Tile>();
                tiles[y, x].InitPosition(x, y);
                tiles[y, x].transform.localPosition = new Vector2(x, y);
            }
        }

        transform.position = new Vector2(-(float)boardSize.x / 2f + 1f / 2f, -(float)boardSize.y / 2 + 1f / 2f);
    }

    void Update()
    {
        if(player.isMoving)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0)) //좌클릭 타일 켜고 끄기
        {
            Vector2Int tileIndex = PointToTileIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (IsValidIndex(tileIndex))
            {
                tiles[tileIndex.y, tileIndex.x].OnOffWall();
            }

        }
        else if (Input.GetMouseButtonDown(1)) //우클릭 이동 위치 선택
        {
            Vector2Int tileIndex = PointToTileIndex(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (IsValidIndex(tileIndex))
            {
                if (tiles[tileIndex.y,tileIndex.x].isWall == true)
                {
                    return;
                }


                RefreshAll();
                endPoint = tileIndex;

                //길찾기
                AStarPathFinding(startPoint, endPoint);

                //경로 그리기
                foreach (Tile t in tilePath)
                {
                    t.printPath();
                }
                if(tilePath.Count > 0) //시작위치 이동
                {
                    startPoint = tilePath[tilePath.Count-1].tilePosition;
                }

                //경로따라 이동
                player.SetMovePath(tilePath);
            }
        }
    }


    void AStarPathFinding(Vector2Int startPos, Vector2Int endPos)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closeList = new List<Tile>();


        Tile startTile = tiles[startPos.y, startPos.x];
        Tile endTile = tiles[endPos.y, endPos.x];
        Tile curTile = null;


        startTile.G = 0;
        startTile.H = (endPos - startPos).magnitude;

        openList.Add(startTile);

        //경로탐색
        while(openList.Count > 0)
        {
            Vector2Int[] dir = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            float minF = float.MaxValue;
            foreach (Tile t in openList)
            {
                if(t.F() < minF)
                {
                    curTile = t;
                    minF = t.F();
                }
            }
            openList.Remove(curTile);
            closeList.Add(curTile);

            //경로 찾음
            if (curTile == endTile)
            {
                break;
            }

            for(int i=0;i<4;i++)
            {
                Vector2Int nextIndex = curTile.tilePosition + dir[i];
                if(IsValidIndex(nextIndex))
                {
                    if (tiles[nextIndex.y, nextIndex.x].isWall || closeList.Contains(tiles[nextIndex.y, nextIndex.x]))
                    {
                        continue;
                    }

                    if(openList.Contains(tiles[nextIndex.y, nextIndex.x]))
                    {
                        if(tiles[nextIndex.y, nextIndex.x].G > curTile.G+1)
                        {
                            tiles[nextIndex.y, nextIndex.x].G = curTile.G + 1;
                            tiles[nextIndex.y, nextIndex.x].parentTile = curTile;
                        }
                    }
                    else
                    {
                        openList.Add(tiles[nextIndex.y, nextIndex.x]);
                        tiles[nextIndex.y, nextIndex.x].G = curTile.G + 1;
                        tiles[nextIndex.y, nextIndex.x].H = (endPos - curTile.tilePosition).magnitude;
                        tiles[nextIndex.y, nextIndex.x].parentTile = curTile;
                    }
                }
            }
        }

        //경로 저장
        if(curTile == endTile)
        {
            while (curTile.parentTile != null)
            {
                tilePath.Add(curTile);
                curTile = curTile.parentTile;
            }
            tilePath.Add(curTile);
        }
        tilePath.Reverse();
    }


    Vector2Int PointToTileIndex(Vector3 mousePoint)
    {
        Vector2Int tileIndex = new Vector2Int();
        tileIndex.x = (int)(mousePoint.x - transform.position.x + 1f / 2f);
        tileIndex.y = (int)(mousePoint.y - transform.position.y + 1f / 2f);
        return tileIndex;
    }

    bool IsValidIndex(Vector2Int tileIndex)
    {
        if (tileIndex.x >= boardSize.x || tileIndex.x < 0 || tileIndex.y >= boardSize.y || tileIndex.y < 0)
        {
            return false;
        }
        return true;
    }

    void RefreshAll()
    {
        tilePath.Clear();
        foreach (Tile t in tiles)
        {
            t.Refresh();
        }
    }
}
