using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public bool isMove = false;
    void Start()
    {
        GameManager.inst.curBoard.curBackGround = this;
    }

    void Update()
    {
        if (isMove)
        {
            transform.position += Vector3.left * (1.5f * Time.deltaTime);
            if (transform.position.x < -4)
            {
                transform.Translate(8, 0, 0);
            }
        }
    }
}
