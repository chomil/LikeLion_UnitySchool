using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public enum CharacterType
{
    Sword,Bow,Magic
}

public class Character : MonoBehaviour
{
    public CharacterType characterType;
    public int level;
    private Animator anim;
    protected int fullBeat = 4;
    void Awake()
    {
        anim = GetComponent<Animator>();

    }

    private void Start()
    {
        switch (characterType)
        {
            case CharacterType.Sword:
                fullBeat = 4;
                break;
            
        }
    }

    protected void Update()
    {
        int curBeat = SoundManager.inst.curBeat;
        int prevBeat = SoundManager.inst.prevBeat;

        if (prevBeat != curBeat)
        {
            Debug.Log(curBeat);
            transform.DOScaleY(0.85f, 0.05f).SetLoops(2, LoopType.Yoyo);
        }
    }


    public void Attack()
    {
        anim.SetTrigger("AttackTrigger");
    }
}
