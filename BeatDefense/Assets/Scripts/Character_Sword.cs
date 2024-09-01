using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Sword : Character
{
    private Monster targetMonster = null;

    protected override void Start()
    {
        base.Start();
        characterType = CharacterType.Sword;
    }

    protected override void Update()
    {
        base.Update();
        
        if (isAttack)
        {
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
        StartCoroutine(AttackCoroutine());
    }

    public IEnumerator AttackCoroutine()
    {
        isAttack = true;
        outline.enabled = true;
        yield return new WaitForSeconds(0.1f);
        if (targetMonster)
        {
            targetMonster.Damaged(attDamage);
        }
        yield return new WaitForSeconds(0.3f);
        
        outline.enabled = false;
        isAttack = false;
    }
    
}
