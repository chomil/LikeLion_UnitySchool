using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Monster : Character
{
    private int roadIndex = 0;

    protected override void Start()
    {
        Vector3 pos = GameManager.inst.curStage.roads[roadIndex].transform.position;
        pos.y = 1;

        transform.position = pos;
    }

    protected override void Update()
    {        
        int curBeat = SoundManager.inst.curBeat;
        int prevBeat = SoundManager.inst.prevBeat;
        if (prevBeat != curBeat)
        {
            Move();
        }
        base.Update();
    }

    void Move()
    {
        roadIndex++;
        if (GameManager.inst.curStage.roads.Count <= roadIndex)
        {
            return;
        }
        Vector3 pos = GameManager.inst.curStage.roads[roadIndex].transform.position;
        pos.y = 1;

        Vector3 dir = pos - transform.position;
        dir.Normalize();

        transform.forward = dir;
        
        transform.DOMove(pos, 0.05f);
    }
    
}
