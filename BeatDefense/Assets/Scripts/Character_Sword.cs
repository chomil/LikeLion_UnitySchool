using System;
using System.Collections;
using System.Collections.Generic;
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
            Attack();
            if (SoundManager.inst.CompareBeat(fullBeat, 3))
            {
            }
        }
        
        targetMonster = null;
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        for (int i = 0; i < monsters.Length; i++)
        {
            Monster curMon = monsters[i].GetComponent<Monster>();
            if (Math.Abs(curMon.transform.position.x - transform.position.x) <= 3 &&
                Math.Abs(curMon.transform.position.z - transform.position.z) <= 3)
            {
                targetMonster = curMon;
                Vector3 lookDir = targetMonster.transform.position - transform.position;
                characterMesh.transform.forward = lookDir;
                break;
            }
        }

        if (targetMonster == null)
        {
            characterMesh.transform.forward = Vector3.back;
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
