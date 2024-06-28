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
            if (damage <= 0.5f && objectType == ObjectType.Enemy)
            {
                SoundManager.instance.PlaySound(hitSounds);
            }
            SoundManager.instance.PlaySound(dieSounds,0.5f);
            if (dieEffect != null)
            {
                Instantiate(dieEffect, transform.position, Quaternion.identity);
            }

            KillObject();
        }
    }

    public void KillObject()
    {
        if (gameObject.CompareTag("Pig"))
        {
            GameManager.instance.curStage.pigCount--;
        }
        GameManager.instance.curStage.AddScore(score);

        ScoreType scoreType = ScoreType.None;
        switch (objectType)
        {
            case ObjectType.Enemy:
                scoreType = ScoreType.PigScore;
                break;
            case ObjectType.StoneWall:
                scoreType = ScoreType.StoneScore;
                break;
            case ObjectType.WoodWall:
                scoreType = ScoreType.WoodScore;
                break;
        }
        GameManager.instance.curStage.DrawScore(transform.position, score, scoreType);
        
        Destroy(gameObject);
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