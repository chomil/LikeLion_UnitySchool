using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;

public enum Elemental
{
    Fire, Grass, Ground, Electric, Water, None
}


public enum Direction
{
    U, D, L, R, None
}


public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private bool isClicked = false;
    public bool isMatched = false;
    public bool isSelected = false;
    public int freezeCnt = 0;
    public GameObject frozenCover;
    public List<Sprite> frozenSprites;
    public GameObject selectedCover;
    public Elemental elemental;
    public Vector2Int tileIndex;
    public Tile[] nearTiles = new Tile[4]; //U,D,L,R;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.inst.curBoard.skill != Skill.None)
        {
            GameManager.inst.curBoard.SelectTile(this);
        }
    }

    public void Pop()
    {
        isClicked = false;
        isMatched = false;
        isSelected = false;
        if (freezeCnt > 0)
        {
            SetFreeze(--freezeCnt);
            selectedCover.SetActive(false);
        }
        else
        {
            GameManager.inst.curBoard.DeleteTile(this);
        }
    }
    
    public void SetFreeze(int num = 3)
    {
        freezeCnt = num;
        if (num > 0)
        {
            if (frozenCover.activeSelf == false)
            {
                frozenCover.SetActive(true);
                frozenCover.transform.localScale = Vector3.zero;
                frozenCover.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
            }
            isMatched = false;
            isClicked = false;
            frozenCover.GetComponent<Image>().sprite = frozenSprites[3-num];
        }
        else
        {
            frozenCover.SetActive(false);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.inst.curBoard.isMoving)
        {
            return;
        }

        if (GameManager.inst.curBoard.skill == Skill.None)
        {
            if (freezeCnt == 0)
            {
                isClicked = true;
                Debug.Log("Pressed");
            }
        }
        else
        {
            GameManager.inst.curBoard.UseSkill(this);
            Debug.Log("Skill");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (GameManager.inst.curBoard.isMoving || freezeCnt>0)
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

            if (targetTile.freezeCnt > 0)
            {
                return;
            }
            Debug.Log("Swap");
            GameManager.inst.curBoard.SwapTile(this, targetTile);
        }
    }
}
