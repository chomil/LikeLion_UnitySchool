using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;


public enum Skill
{
    Punch,Heal,Meteor,Vertical,Horizontal
}

public class Player : MonoBehaviour
{
    public int maxHp;
    public int hp;
    private Animator animator;

    public GameObject hpBar;
    public GameObject hpBarDelay;

    public TextMeshProUGUI hpText;

    public GameObject magicPos;
    public GameObject magicPrefab;
    private List<Elemental> magicList = new List<Elemental>();
    
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
        
        UpdateHpText();
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
        if (damage == 0 || hp == 0)
        {
            return;
        }
        
        hp = Math.Clamp(hp - damage, 0, maxHp);
        hpBar.transform.localScale = new Vector3((float)hp / (float)maxHp, 1, 1);
        hpBarDelay.transform.DOScale(hpBar.transform.localScale,1f);

        if (hp == 0)
        {
            animator.SetTrigger("TriggerDie");
        }
        else
        {
            animator.SetTrigger("TriggerHit");
        }
        
        UpdateHpText();
    }

    public void Run(bool isRun)
    {
        animator.SetBool("isRun", isRun);
    }

    public void AddAttack(Elemental elemental)
    {
        magicList.Add(elemental);
        if (magicList.Count == 1)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    public IEnumerator AttackCoroutine()
    {
        animator.SetTrigger("TriggerAttack");
        yield return new WaitForSeconds(0.5f);
        while (magicList.Count > 0)
        {
            Elemental elemental = magicList[0];
            magicList.RemoveAt(0);
            MagicEffect magic = Instantiate(magicPrefab, magicPos.transform.position, quaternion.identity).GetComponent<MagicEffect>();
            magic.Initialize(elemental);
            yield return new WaitForSeconds(0.04f);
        }
    }
    
    public void AttackEnd()
    {
        animator.SetTrigger("TriggerIdle");
    }

    public void UpdateHpText()
    {
        hpText.text = $"HP  {hp}/{maxHp}";
    }
}
