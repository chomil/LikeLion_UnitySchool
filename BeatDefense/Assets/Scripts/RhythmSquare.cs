using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmSquare : MonoBehaviour
{
    private Animation popAnim;
    void Start()
    {
        popAnim = GetComponent<Animation>();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (GameManager.inst.curStage.isPlaying)
            {
                popAnim.Play();
            }
        }
    }
}
