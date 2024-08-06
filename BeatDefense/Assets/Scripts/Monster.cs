using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Animator anim;
    private int prevBeat = 0;
    void Awake()
    {
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        int curBeat = SoundManager.inst.CountBeat(4);

        if (prevBeat != curBeat)
        {
            transform.DOScaleY(0.8f, 0.05f).SetLoops(2, LoopType.Yoyo);
        }
        
        prevBeat = curBeat;
    }
}
