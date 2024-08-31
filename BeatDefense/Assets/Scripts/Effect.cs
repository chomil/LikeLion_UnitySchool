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
        Debug.Log("Spawn" + transform.ToString());
        if (spawnSfx)
        {
            SoundManager.inst.PlaySound(spawnSfx);
        }
    }

    public void DestroyEffect()
    {
        if (destroySfx)
        {
            SoundManager.inst.PlaySound(destroySfx);
        }
        Destroy(gameObject);
    }
}
