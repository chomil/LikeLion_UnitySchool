using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space)) //Test moving
        {
            animator.SetBool("isRun", true);
        }
        if (Input.GetKeyUp(KeyCode.Space)) //Test moving
        {
            animator.SetBool("isRun", false);
        }
    }
}
