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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            popAnim.Play();
        }
    }
}
