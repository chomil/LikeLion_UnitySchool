using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isMoving = false;
    List<Tile> tilePath = new List<Tile>();

    public void SetMovePath(List<Tile> tilePath)
    {
        this.tilePath = tilePath;
    }

    void Update()
    {
        if(tilePath.Count > 0) 
        {
            isMoving = true;
            Move();
        }
        else
        {
            isMoving = false;
        }
    }

    void Move()
    {
        transform.localPosition = Vector2.MoveTowards(transform.localPosition, tilePath[0].transform.localPosition, 10f*Time.deltaTime);
        if(transform.localPosition == tilePath[0].transform.localPosition)
        {
            tilePath.RemoveAt(0);
        }
    }
}
