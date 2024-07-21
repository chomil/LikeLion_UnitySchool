using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHp;
    public int hp;
    private Animator animator;

    public GameObject hpBar;
    public GameObject hpBarDelay;
    
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        maxHp = GameManager.inst.gameData.maxHp;
        hp = maxHp;
        hpBar.transform.localScale = Vector3.one;
        hpBarDelay.transform.localScale = Vector3.one;

        GameManager.inst.curBoard.curPlayer = this;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetDamage(5);
        }
    }

    public void GetDamage(int damage)
    {
        hp = Math.Clamp(hp - damage, 0, maxHp);
        hpBar.transform.localScale = new Vector3((float)hp / (float)maxHp, 1, 1);
        hpBarDelay.transform.DOScale(hpBar.transform.localScale,1f);
        
        
        animator.SetTrigger("TriggerHit");
    }

    public void Run(bool isRun)
    {
        animator.SetBool("isRun", isRun);
    }
}
