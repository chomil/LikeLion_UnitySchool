using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float moveSpeed = 1f;
    void Update()
    {
        transform.position += Vector3.left * (Time.deltaTime * moveSpeed);
        if (transform.position.x < -100f)
        {
            transform.position = new Vector3(100, transform.position.y, transform.position.z);
        }
    }
}
