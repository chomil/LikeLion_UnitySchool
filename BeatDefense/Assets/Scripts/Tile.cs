using System.Collections;
using System.Collections.Generic;
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

    private void SetSelect(bool _isSelect)
    {
        isSelect = _isSelect;
        selectCover.gameObject.SetActive(isSelect);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetSelect(true);

        Character spawnCharacter = GameManager.inst.curStage.spawnCharacter;
        if (spawnCharacter)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = 1;
            spawnCharacter.transform.position = spawnPos;
            spawnCharacter.SetSelect(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetSelect(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelect(false);
        
        Character spawnCharacter = GameManager.inst.curStage.spawnCharacter;
        if (canSelect&& spawnCharacter)
        {
            Vector3 spawnPos = transform.position;
            spawnPos.y = 1;
            spawnCharacter.SetSelect(false);
            spawnCharacter.SetTemp(false);
            GameManager.inst.curStage.spawnCharacter = null;
        }
    }
}
