using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feel;
using Shapes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector2Int tilePos;
    public bool isRoad = false;
    public bool isStart = false;
    public bool canSelect = false;
    private bool isSelect = false;
    public ShapeRenderer selectCover = null;


    void Start()
    {
        tilePos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
        GameManager.inst.curStage.tiles.Add(tilePos,this);
        
        
        selectCover.Color = canSelect == false ? Color.red : Color.white;
    }

    public void SetCanSelect(bool _canSelect)
    {
        canSelect = _canSelect;
        selectCover.Color = canSelect == false ? Color.red : Color.white;
    }
    public void SetSelect(bool _isSelect)
    {
        isSelect = _isSelect;
        selectCover.gameObject.SetActive(isSelect);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Character spawnCharacter = GameManager.inst.curStage.spawnCharacter;
        if (spawnCharacter)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = 1.2f;
            spawnCharacter.transform.position = spawnPos;
            spawnCharacter.SetSelect(true);
            
            SetSelect(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Character spawnCharacter = GameManager.inst.curStage.spawnCharacter;
        if (spawnCharacter)
        {
            SetSelect(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.inst.curStage.SpawnCharacterOnTile(this))
        {
            SetSelect(false);
        }
    }
}
