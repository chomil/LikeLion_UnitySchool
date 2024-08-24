using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Shapes;
using UnityEngine;
using UnityEngine.EventSystems;


public enum CharacterType
{
    Sword,Bow,Magic, Monster
}

public abstract class Character : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,IPointerExitHandler
{
    public CharacterType characterType;
    public int level;
    private Animator anim;
    protected int fullBeat = 4;

    public GameObject range;
    private Rectangle rangeRectangle;
    private Outline outline;
    public bool isSelected = false;
    public bool isTemp = false;
    
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        if (range)
        {
            rangeRectangle = range.GetComponent<Rectangle>();
        }

        outline = GetComponent<Outline>();
    }

    protected virtual void Start()
    {
        switch (characterType)
        {
            case CharacterType.Sword:
                fullBeat = 4;
                break;
            
        }
    }

    protected virtual void Update()
    {
        int curBeat = SoundManager.inst.curBeat;
        int prevBeat = SoundManager.inst.prevBeat;

        if (prevBeat != curBeat)
        {
            transform.DOScaleY(0.85f, 0.05f).SetLoops(2, LoopType.Yoyo);
        }

        if (isSelected)
        {
            rangeRectangle.DashOffset += Time.deltaTime;
        }
    }

    public void SetTemp(bool _isTemp)
    {
        isTemp = _isTemp;
        if (isTemp)
        {
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<Collider>().enabled = true;
        }
    }


    public void Attack()
    {
        anim.SetTrigger("AttackTrigger");
    }

    public void SetSelect(bool _isSelect)
    {
        isSelected = _isSelect;
        range.SetActive(isSelected);
        outline.enabled = _isSelect;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name+" Click");
        GameManager.inst.curStage.SetSelectCharacter(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected == false)
        {
            outline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected == false)
        {
            outline.enabled = false;
        }
    }
}
