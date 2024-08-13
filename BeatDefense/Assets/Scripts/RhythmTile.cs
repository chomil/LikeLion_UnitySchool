using System.Collections;
using System.Collections.Generic;
using Shapes;
using Unity.Mathematics;
using UnityEngine;

public class RhythmTile : MonoBehaviour
{
    private Rectangle cover;
    void Awake()
    {
        cover = GetComponent<Rectangle>();
    }

    void Update()
    {
        int xIndex = ((int)transform.position.x+1) / 2;
        int zIndex = ((int)transform.position.z+1) / 2;
        bool even = math.abs(zIndex + xIndex) % 2 == 0;
        int beat = SoundManager.inst.curBeat;
        
        cover.enabled = (beat % 2 == 0)^even;
        
    }
}
