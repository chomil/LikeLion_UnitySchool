using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Magician : Character
{
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
    }

    public override void Shoot()
    {
        base.Shoot();
        StartCoroutine(AttackCoroutine());
    }
    public IEnumerator AttackCoroutine()
    {
        outline.enabled = true;
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
                curMon.SetFreeze(1-0.2f*level,1f);
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
