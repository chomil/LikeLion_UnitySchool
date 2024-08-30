using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Bow : Character
{
    private Monster targetMonster = null;
    
    protected override void Start()
    {
        base.Start();
        characterType = CharacterType.Bow;
    }
    protected override void Update()
    {
        base.Update();
        
        if (isAttack)
        {
            if (targetMonster)
            {
                Vector3 lookDir = targetMonster.transform.position - transform.position;
                characterMesh.transform.forward = lookDir;
            }
            return;
        }
        
        targetMonster = null;

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
                targetMonster = curMon;
                Vector3 lookDir = targetMonster.transform.position - transform.position;
                characterMesh.transform.forward = lookDir;
                break;
            }
        }
    }

    public override void Attack()
    {
        base.Attack();
        
        isAttack = true;
    }

    public override void Shoot()
    {
        base.Shoot();
        StartCoroutine(AttackCoroutine());
    }


    public override void CancelAttack()
    {
        base.CancelAttack();
    }

    public IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        if (targetMonster)
        {
            targetMonster.Damaged(1);
        }
        yield return new WaitForSeconds(0.3f);
        isAttack = false;
    }
    
}
