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
    public int prefabIndex;
    
    public CharacterType characterType;
    public int level;
    public int price;
    public int upgradePrice;
    public int attDamage;
    protected Animator anim;

    public GameObject characterMesh;
    [HideInInspector] public Outline outline;
    public GameObject range;
    protected Disc rangeDisc;
    protected bool isSelected = false;
    public bool canInteractive = true;

    private Collider collider;
    protected bool isAttack = false;
    
    
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();

        if (range)
        {
            rangeDisc = range.GetComponent<Disc>();
        }

        outline = characterMesh.GetComponent<Outline>();
        collider = GetComponent<Collider>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        int curBeat = SoundManager.inst.curBeat;
        int prevBeat = SoundManager.inst.prevBeat;

        if (prevBeat != curBeat)
        {
            characterMesh.transform.DOScaleY(0.75f, 0.05f).SetLoops(2, LoopType.Yoyo);
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
            collider.enabled = true;
        }
        else
        {
            collider.enabled = false;
        }
    }


    public virtual void Attack()
    {
        anim.SetTrigger("AttackTrigger");
    }

    public virtual void Shoot()
    {
        anim.SetTrigger("ShootTrigger");
    }
    public virtual void CancelAttack()
    {
        anim.SetTrigger("IdleTrigger");
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

    private void OnDestroy()
    {
        characterMesh.transform.DOKill();
    }
}
