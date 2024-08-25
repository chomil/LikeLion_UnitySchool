using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sequence = Unity.VisualScripting.Sequence;

public class Monster : Character
{
    private int roadIndex = 0;
    private int Hp = 3;
    

    protected override void Start()
    {
        Vector3 pos = GameManager.inst.curStage.roads[roadIndex].transform.position;
        pos.y = 1;

        transform.position = pos;
    }

    public void Damaged(int damage)
    {
        Hp -= damage;
        Hp = Hp < 0 ? 0 : Hp;

        if (Hp == 0)
        {
            //Die
        }
    }

    protected override void Update()
    {        
        base.Update();
        
        int curBeat = SoundManager.inst.curBeat;
        int prevBeat = SoundManager.inst.prevBeat;
        if (prevBeat != curBeat)
        {
            Move();
        }
    }

    void Move()
    {
        Tile prevTile = GameManager.inst.curStage.roads[roadIndex];
        prevTile.characterOnTile = null;
        
        roadIndex++;
        if (GameManager.inst.curStage.roads.Count <= roadIndex)
        {
            transform.DOKill(false);
            Destroy(gameObject);
            return;
        }
        
        Tile nextTile = GameManager.inst.curStage.roads[roadIndex];
        nextTile.characterOnTile = this;
        Vector3 pos = nextTile.transform.position;
        pos.y = 1;

        Vector3 dir = pos - transform.position;
        dir.Normalize();

        transform.forward = dir;
        
        transform.DOMove(pos, 0.05f);
    }
    
}
