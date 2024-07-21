using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Elemental elemental;
    
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
        hp = maxHp;
        hpBar.transform.localScale = Vector3.one;
        hpBarDelay.transform.localScale = Vector3.one;
        
        
        GameManager.inst.curBoard.curMonster = this;
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetDamage(5);
        }
    }

    public void GetDamage(int damage , Elemental hitElemental = Elemental.None)
    {
        damage = (int)(MultipleDamage(hitElemental)*damage);
        if (damage == 0)
        {
            return;
        }
        
        hp = Math.Clamp(hp - damage, 0, maxHp);
        hpBar.transform.localScale = new Vector3((float)hp / (float)maxHp, 1, 1);
        hpBarDelay.transform.DOScale(hpBar.transform.localScale,1f);
        
        if (hp > 0)
        {
            animator.SetTrigger("TriggerHit");
        }
        else
        {
            animator.SetTrigger("TriggerDie");
        }
    }

    private float MultipleDamage(Elemental hitElemental)
    {
        if (elemental == (Elemental)(((int)hitElemental + 1)%5))
        {
            return 5f;
        }
        else if ((Elemental)(((int)elemental+ 1)%5) == hitElemental)
        {
            return 0f;
        }
        else
        {
            return 1f;
        }
    }
}
