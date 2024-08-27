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
    protected Animator anim;
    protected int fullBeat = 4;

    public GameObject characterMesh;
    public GameObject range;
    protected Disc rangeDisc;
    protected Outline outline;
    protected bool isSelected = false;
    protected bool canInteractive = true;
    
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        if (range)
        {
            rangeDisc = range.GetComponent<Disc>();
        }

        outline = characterMesh.GetComponent<Outline>();
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
            characterMesh.transform.DOScaleY(0.85f, 0.05f).SetLoops(2, LoopType.Yoyo);
        }

        if (isSelected)
        {
            rangeDisc.DashOffset += Time.deltaTime;
        }
    }

    public void SetInteractive(bool _canInteractive)
    {
        canInteractive = _canInteractive;
        if (canInteractive)
        {
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            GetComponent<Collider>().enabled = false;
        }
    }


    protected virtual void Attack()
    {
        anim.SetTrigger("AttackTrigger");
    }

    public void SetSelect(bool _isSelect)
    {
        if (canInteractive)
        {
            isSelected = _isSelect;
            range.SetActive(isSelected);
            outline.enabled = _isSelect;
        }
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
