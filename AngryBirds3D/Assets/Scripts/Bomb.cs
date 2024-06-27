using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject owner;
    public GameObject effect;
    private float radius;
    private float bombPower = 20f;
    public AudioClip bombSound;

    private void Awake()
    {
        radius = GetComponent<SphereCollider>().radius;
        Instantiate(effect, transform.position, Quaternion.identity);
        SoundManager.instance.PlaySound(bombSound, 0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner)
        {
            return;
        }
        if (other.gameObject.CompareTag("Bird"))
        {
            Bird otherBird =  other.gameObject.GetComponent<Bird>();

            if (otherBird.isMoving == false)
            {
                return;
            }
            
            if (otherBird.type == BirdType.Bomb)
            {
                otherBird.OnDamage();
            }
        }

        Rigidbody otherRigid = other.GetComponent<Rigidbody>();
        if (otherRigid)
        {
            Vector3 dir = other.transform.position - transform.position;
            float power =  Math.Max(1f - dir.magnitude / radius, 0.5f);
            dir.Normalize();
            otherRigid.AddForce(otherRigid.mass * dir * bombPower* power, ForceMode.Impulse);
            
            MapObject mapObject = other.GetComponent<MapObject>();
            if(mapObject)
            {
                mapObject.OnDamage(power);
            }
        }
        
        Destroy(gameObject);
    }
}