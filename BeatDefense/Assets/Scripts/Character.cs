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

public abstract class Character : MonoBehaviour, IPointerClickHandler
{
    public CharacterType characterType;
    public int level;
    private Animator anim;
    protected int fullBeat = 4;

    public GameObject range;
    private Rectangle rangeRectangle;
    private bool isSelected = false;
    
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        if (range)
        {
            rangeRectangle = range.GetComponent<Rectangle>();
        }
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


    public void Attack()
    {
        anim.SetTrigger("AttackTrigger");
    }

    public void SetSelect(bool _isSelect)
    {
        isSelected = _isSelect;
        range.SetActive(isSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(name+" Click");
        SetSelect(!isSelected);
    }
}
