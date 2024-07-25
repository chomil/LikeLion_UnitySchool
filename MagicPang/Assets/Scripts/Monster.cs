using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum MonsterType
{
    Rabbit, Slime
}
public class Monster : MonoBehaviour
{
    public MonsterType monsterType;
    public Elemental elemental;

    public int level = 1;
    public int maxHp;
    public int hp;
    private Animator animator;

    public MonsterStatus statusWindow;

    public GameObject hitPos;
    public GameObject dieEffect;

    public bool isMove = false;

    public List<AudioClip> attackSfxList;
    public List<AudioClip> hitSfxList;
    public List<AudioClip> dieSfxList;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameManager.inst.curBoard.curMonster = this;
        
        statusWindow.elementalImage.sprite = GameManager.inst.elementalSprites[(int)elemental];
        statusWindow.lvText.text = $"Lv{level}";

        maxHp = level * 10;
        
        if (monsterType == MonsterType.Rabbit)
        {
            maxHp = level * 15;
        }
        hp = maxHp;
        statusWindow.UpdateHp(hp,maxHp);
    }

    void Update()
    {
        if (isMove)
        {
            transform.position += Vector3.left * (1.5f * Time.deltaTime);
            if (transform.position.x <= 1.2f)
            {
                transform.position = new Vector3(1.2f, transform.position.y, transform.position.z);
                isMove = false;
            }
        }
    }

    public void Die()
    {
        SoundManager.inst.PlaySound(dieSfxList, 0.3f);
        Destroy(gameObject);
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
        
        statusWindow.UpdateHp(hp,maxHp);
        
        SoundManager.inst.PlaySound(hitSfxList, 0.5f);
        if (hp <= 0)
        {
            animator.SetTrigger("TriggerDie");
        }
        else
        {
            animator.SetTrigger("TriggerHit");
        }
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
    

    public IEnumerator AttackCoroutine()
    {
        SoundManager.inst.PlaySound(attackSfxList);
        animator.SetTrigger("TriggerAttack");
        yield return new WaitForSeconds(0.5f);
        GameManager.inst.curBoard.curPlayer.GetDamage(level);
        if (monsterType == MonsterType.Slime)
        {
            GameManager.inst.curBoard.FreezeTiles(1+(level/10),2);
        }
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