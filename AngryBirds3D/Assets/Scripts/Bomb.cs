using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject owner;
    private float radius;
    public float bombPower = 50f;

    private void Start()
    {
        radius = GetComponent<SphereCollider>().radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner)
        {
            return;
        }

        Rigidbody otherRigid = other.GetComponent<Rigidbody>();
        if (otherRigid)
        {
            Vector3 dir = other.transform.position - transform.position;
            float power = bombPower * (Math.Max(1f - dir.magnitude / radius, 0f));
            dir.Normalize();
            otherRigid.AddForce(dir * power, ForceMode.Impulse);
        }
        
        Destroy(gameObject);
    }
}