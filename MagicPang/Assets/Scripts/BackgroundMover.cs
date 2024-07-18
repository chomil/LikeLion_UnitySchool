using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) //Test moving
        {
            transform.position += Vector3.left * Time.deltaTime;
            if (transform.position.x < -4)
            {
                transform.Translate(8, 0, 0);
            }
        }
    }
}
