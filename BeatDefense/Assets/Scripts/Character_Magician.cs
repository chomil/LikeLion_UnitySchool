using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Magician : Character
{
    public Effect chargeMagicPrefab;
    public Effect shootMagicPrefab;
    protected override void Start()
    {
        base.Start();
        characterType = CharacterType.Magic;
    }
    protected override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        base.Attack();

        Effect magic = Instantiate(chargeMagicPrefab, transform); //Spawn Magic
        if (level >= 3)
        {
            magic.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }

    public override void Shoot()
    {
        base.Shoot();
        StartCoroutine(AttackCoroutine());
    }
    public IEnumerator AttackCoroutine()
    {
        outline.enabled = true;
        Effect magic = Instantiate(shootMagicPrefab, transform);
        if (level >= 3)
        {
            magic.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        yield return new WaitForSeconds(0.1f);
        foreach (Monster curMon in GameManager.inst.curStage.monsters)
        {
            if (!curMon)
            {
                continue;
            }
            if (curMon.Hp == 0)
            {
                continue;
            }
            if (Vector3.Magnitude( curMon.transform.position - transform.position) <= rangeDisc.Radius)
            {
                curMon.Damaged(attDamage);
                curMon.SetFreeze(1-0.2f*level,2f);
            }
        }
        yield return new WaitForSeconds(0.3f);
        outline.enabled = false;
        isAttack = false;
    }

    public override void CancelAttack()
    {
        base.CancelAttack();
    }
}
