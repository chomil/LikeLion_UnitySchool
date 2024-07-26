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
    Fire,
    Grass,
    Ground,
    Electric,
    Water,
    None
}


public enum Direction
{
    U,
    D,
    L,
    R,
    None
}

public enum TileSkill
{
    Vertical,
    Horizontal,
    AllDir,
    None
}


public class Tile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private bool isClicked = false;
    public bool isMatched = false;
    public bool isSelected = false;

    public int freezeCnt = 0;
    public GameObject frozenCover;
    public List<Sprite> frozenSprites;

    public TileSkill tileSkill = TileSkill.None;
    public GameObject skillCover;
    public List<Sprite> skillSprites;

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

    public void Matching()
    {
        if (isMatched)
        {
            return;
        }

        isMatched = true;

        if (freezeCnt == 0)
        {
            if (tileSkill == TileSkill.Vertical)
            {
                Tile nextTile = nearTiles[(int)Direction.U];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.U];
                }

                nextTile = nearTiles[(int)Direction.D];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.D];
                }
            }
            else if (tileSkill == TileSkill.Horizontal)
            {
                Tile nextTile = nearTiles[(int)Direction.L];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.L];
                }

                nextTile = nearTiles[(int)Direction.R];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.R];
                }
            }
            else if (tileSkill == TileSkill.AllDir)
            {
                Tile nextTile = nearTiles[(int)Direction.U];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.U];
                }

                nextTile = nearTiles[(int)Direction.D];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.D];
                }

                nextTile = nearTiles[(int)Direction.L];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.L];
                }

                nextTile = nearTiles[(int)Direction.R];
                while (nextTile != null)
                {
                    nextTile.Matching();
                    nextTile = nextTile.nearTiles[(int)Direction.R];
                }
            }

            tileSkill = TileSkill.None;
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
            if (tileSkill == TileSkill.Vertical)
            {
            }
            else if (tileSkill == TileSkill.Horizontal)
            {
            }
            else if (tileSkill == TileSkill.AllDir)
            {
            }

            GameManager.inst.curBoard.DeleteTile(this);
        }
    }

    public void SetSkill(TileSkill skill)
    {
        if (isMatched)
        {
            return;
        }

        tileSkill = skill;
        if (tileSkill == TileSkill.None)
        {
            skillCover.SetActive(false);
        }
        else
        {
            skillCover.SetActive(true);
            Image skillImage = skillCover.GetComponent<Image>();
            if (skill == TileSkill.Vertical)
            {
                skillImage.sprite = skillSprites[0];
            }
            else if (skill == TileSkill.Horizontal)
            {
                skillImage.sprite = skillSprites[1];
            }
            else if (skill == TileSkill.AllDir)
            {
                skillImage.sprite = skillSprites[2];
            }
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
            frozenCover.GetComponent<Image>().sprite = frozenSprites[3 - num];
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
        if (GameManager.inst.curBoard.isMoving || freezeCnt > 0)
        {
            isClicked = false;
            return;
        }

        if (isClicked == true)
        {
            Vector2 dir = eventData.position - new Vector2(transform.position.x, transform.position.y);
            Tile targetTile = null;
            if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            {
                targetTile = dir.y > 0 ? nearTiles[(int)Direction.U] : nearTiles[(int)Direction.D];
            }
            else
            {
                targetTile = dir.x > 0 ? nearTiles[(int)Direction.R] : nearTiles[(int)Direction.L];
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