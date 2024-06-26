using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject owner;
    private float radius;
    public float bombPower = 20f;

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

        MapObject mapObject = other.GetComponent<MapObject>();
        Rigidbody otherRigid = other.GetComponent<Rigidbody>();
        if (otherRigid && mapObject)
        {
            Vector3 dir = other.transform.position - transform.position;
            float power =  Math.Max(1f - dir.magnitude / radius, 0f);
            dir.Normalize();
            otherRigid.AddForce(otherRigid.mass * dir * bombPower* power, ForceMode.Impulse);
            mapObject.OnDamage(power);
        }
        
        Destroy(gameObject);
    }
}