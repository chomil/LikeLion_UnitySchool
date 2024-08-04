using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RhythmCamera : MonoBehaviour
{
    private int prevBeat = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int curBeat = SoundManager.inst.CountBeat(4);
        if (prevBeat != curBeat)
        {
            if (curBeat % 4 == 0)
            {
                transform.DOMove(transform.position + transform.forward, 0.08f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutExpo);
            }
            else
            {
                transform.DOMove(transform.position + transform.forward*0.25f, 0.08f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutExpo);
            }
        }
        prevBeat = curBeat;
    }
}
