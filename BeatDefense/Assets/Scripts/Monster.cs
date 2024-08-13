using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Animator anim;
    private int roadIndex = 0;
    
    void Awake()
    {
        anim = GetComponent<Animator>();

    }

    private void Start()
    {
        Vector3 pos = GameManager.inst.curStage.roads[roadIndex].transform.position;
        pos.y = 1;

        transform.position = pos;
    }

    void Update()
    {
        int curBeat = SoundManager.inst.curBeat;
        int prevBeat = SoundManager.inst.prevBeat;

        if (prevBeat != curBeat)
        {
            transform.DOScaleY(0.8f, 0.05f).SetLoops(2, LoopType.Yoyo);

            Move();
        }
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
        transform.DOMove(pos, 0.05f);
    }
    
}
