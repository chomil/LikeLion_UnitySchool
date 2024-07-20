using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;

public enum Elemental
{
    Fire, Grass, Ground, Electric, Water, None
}


public enum Direction
{
    U, D, L, R, None
}


public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public bool isClicked = false;
    public bool isMoving = false;
    public bool isMatched = false;
    public Elemental elemental;
    public Vector2Int tileIndex;
    public Tile[] nearTiles = new Tile[4]; //U,D,L,R;
    
    void Start()
    {
        
    }

    void Update()
    {
    }
    public void Falling()
    {
    }
    
    
    public void Poping()
    {
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.inst.curBoard.isMoving)
        {
            return;
        }
        
        isClicked = true;
        Debug.Log("Pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.inst.curBoard.isMoving)
        {
            isClicked = false;
            return;
        }
        if (isClicked == true)
        {
            Vector2 dir = eventData.position - new Vector2(transform.position.x,transform.position.y);
            Tile targetTile = null;
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                targetTile = dir.y > 0 ? nearTiles[0] : nearTiles[1];
            }
            else
            {
                targetTile = dir.x > 0 ? nearTiles[3] : nearTiles[2];
            }
            isClicked = false;

            Debug.Log("Swap");
            GameManager.inst.curBoard.SwapTile(this, targetTile);
        }
    }

}
