using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public AudioClip spawnSfx;
    public AudioClip destroySfx;

    public void Start()
    {
        if (spawnSfx)
        {
            SoundManager.inst.PlaySound(spawnSfx);
        }
    }

    public void DestroyEffect(float delay = 0f)
    {
        StartCoroutine(DestroyCoroutine(delay));
    }

    IEnumerator DestroyCoroutine(float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        if (destroySfx)
        {
            SoundManager.inst.PlaySound(destroySfx);
        }
        Destroy(gameObject);
    }
}
