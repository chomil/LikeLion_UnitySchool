using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ObjectType
{
    WoodWall,
    StoneWall,
    Enemy,
    Tnt
}

public class MapObject : MonoBehaviour
{
    public ObjectType objectType;
    private bool isNoHit = true;
    private float hp = 1f;
    public GameObject dieEffect;
    public int score = 0;
    
    public List<AudioClip> hitSounds;
    public List<AudioClip> dieSounds;

    public void Awake()
    {
        hp = GetComponent<Rigidbody>().mass;
        
        //생성 후 n초간 무적시간
        StartCoroutine(NoHitTime(1f));
    }

    public void OnDamage(float damage = 1f)
    {
        hp -= damage;
        if (damage >= 0.5f)
        {
            SoundManager.instance.PlaySound(hitSounds);
        }
        if (hp <= 0)
        {
            SoundManager.instance.PlaySound(dieSounds,0.5f);
            if (dieEffect != null)
            {
                Instantiate(dieEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (gameObject.CompareTag("Pig"))
        {
            GameManager.instance.curStage.pigCount--;
        }
        GameManager.instance.curStage.AddScore(score);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isNoHit)
        {
            return;
        }

        Vector3 impulse = other.impulse;
        float impulseMagnitude = impulse.magnitude;
        OnDamage(impulseMagnitude / 10f);
        
    }


    IEnumerator NoHitTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isNoHit = false;
    }
    
}