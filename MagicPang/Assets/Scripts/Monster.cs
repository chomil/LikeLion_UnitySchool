using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Elemental elemental;

    public int level = 1;
    public int maxHp;
    public int hp;
    private Animator animator;

    public GameObject hpBar;
    public GameObject hpBarDelay;
    
    public TextMeshProUGUI hpText;
    public Image elementalImage;
    
    public TextMeshProUGUI lvText;

    public GameObject hitPos;

    public bool isMove = false;


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
        
        UpdateHpText();
        elementalImage.sprite = GameManager.inst.elementalSprites[(int)elemental];

        lvText.text = $"Lv{level}";
    }

    void Update()
    {
        if (isMove)
        {
            transform.position += Vector3.left * Time.deltaTime;
            if (transform.position.x <= 1.2f)
            {
                transform.position = new Vector3(1.2f, transform.position.y, transform.position.z);
                isMove = false;
            }
        }
    }

    public void GetDamage(Elemental hitElemental = Elemental.None)
    {
        int damage = 2;
        damage = (int)(MultipleDamage(hitElemental) * damage);
        if (damage == 0)
        {
            return;
        }

        hp = Math.Clamp(hp - damage, 0, maxHp);
        hpBar.transform.localScale = new Vector3((float)hp / (float)maxHp, 1, 1);
        hpBarDelay.transform.DOScale(hpBar.transform.localScale, 1f);

        if (hp <= 0)
        {
            animator.SetTrigger("TriggerDie");
        }
        else
        {
            animator.SetTrigger("TriggerHit");
        }
        
        UpdateHpText();
    }

    public float MultipleDamage(Elemental hitElemental)
    {
        if (elemental == (Elemental)(((int)hitElemental + 1) % 5))
        {
            return 3f;
        }
        else if ((Elemental)(((int)elemental + 1) % 5) == hitElemental)
        {
            return 0f;
        }
        else
        {
            return 1f;
        }
    }
    
    public void UpdateHpText()
    {
        hpText.text = $"HP  {hp}/{maxHp}";
    }


    public IEnumerator AttackCoroutine()
    {
        animator.SetTrigger("TriggerAttack");
        yield return new WaitForSeconds(0.5f);
        GameManager.inst.curBoard.curPlayer.GetDamage(10);
        yield return new WaitForSeconds(0.2f);
    }
    
    public IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        isMove = true;
        GameManager.inst.curBoard.curPlayer.Run(true);
        GameManager.inst.curBoard.curBackGround.isMove = true;
        yield return new WaitWhile(()=>isMove);
        GameManager.inst.curBoard.curPlayer.Run(false);
        GameManager.inst.curBoard.curBackGround.isMove = false;
    }
}