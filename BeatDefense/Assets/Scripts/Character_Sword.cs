using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Sword : Character
{
    private Monster targetMonster = null;
    
    protected override void Update()
    {
        base.Update();
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (SoundManager.inst.CompareBeat(fullBeat, 3))
            {
                Attack();
            }
            else
            {
                
            }
        }
        
        targetMonster = null;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        for (int i = 0; i < monsters.Length; i++)
        {
            Monster curMon = monsters[i].GetComponent<Monster>();
            if (curMon.Hp == 0)
            {
                continue;
            }
            if (Math.Abs(curMon.transform.position.x - transform.position.x) <= 3 &&
                Math.Abs(curMon.transform.position.z - transform.position.z) <= 3)
            {
                targetMonster = curMon;
                Vector3 lookDir = targetMonster.transform.position - transform.position;
                characterMesh.transform.forward = lookDir;
                break;
            }
        }
    }

    protected override void Attack()
    {
        base.Attack();
        if (targetMonster)
        {
            targetMonster.Damaged(1);
        }
    }
    
}
