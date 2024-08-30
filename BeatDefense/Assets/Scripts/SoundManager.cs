using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;
    public AudioSource sfxAudioSource;
    public AudioSource bgmAudioSource;
    public float bgmBpm = 105f;
    public float bgmVol = 0.2f;
    public int prevBeat = 0;
    public int curBeat = 0;
    public float curBeatFloat = 0;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip)
        {
            sfxAudioSource.PlayOneShot(clip, volume * GameManager.inst.gameData.sfxVol);
        }
    }

    public void PlaySound(List<AudioClip> clips, float volume = 1f)
    {
        if (clips.Count > 0)
        {
            int randomIndex = Random.Range(0, clips.Count);
            sfxAudioSource.PlayOneShot(clips[randomIndex], volume * GameManager.inst.gameData.sfxVol);
        }
    }

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        bgmVol = volume;
        if (bgmAudioSource.clip == clip)
        {
            bgmAudioSource.volume = volume * GameManager.inst.gameData.bgmVol;
        }
        else
        {
            StartCoroutine(ChangeBgm(clip, volume));
        }
    }

    private IEnumerator ChangeBgm(AudioClip nextClip, float volume)
    {
        float startVol = bgmAudioSource.volume;
        float curVol = bgmAudioSource.volume;
        float time = 0f;

        while (curVol > 0)
        {
            time += Time.deltaTime;
            curVol = math.lerp(startVol, 0, time * 2);
            bgmAudioSource.volume = curVol;
            yield return null;
        }

        SetBgmVolume(0);

        float targetVol = volume * GameManager.inst.gameData.bgmVol;
        curVol = 0;
        time = 0;
        bgmAudioSource.clip = nextClip;
        bgmAudioSource.Play();

        prevBeat = -1;
        curBeat = 0;
        curBeatFloat = 0f;

        while (curVol < targetVol)
        {
            time += Time.deltaTime;
            curVol = math.lerp(0, targetVol, time * 2);
            bgmAudioSource.volume = curVol;
            yield return null;
        }

        bgmAudioSource.volume = targetVol;
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void SetSfxVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    public void SetBgmVolume(float volume)
    {
        bgmAudioSource.volume = bgmVol * volume;
    }

    public void Update()
    {
        CountBeat();
    }

    public void LateUpdate()
    {
        prevBeat = curBeat;
    }

    private void CountBeat()
    {
        float bps = bgmBpm / 60f;
        float beat = bgmAudioSource.time * bps;

        int cnt = (int)beat % 4; //4beat base
        curBeat = cnt;
        curBeatFloat = (beat - (int)beat) + cnt;
    }

    public bool CompareBeat(int fullBeat, int checkBeat, float tolerance = 0.3f)
    {
        float bps = bgmBpm / 60f * ((float)fullBeat / 4f);
        float beat = bgmAudioSource.time * bps;
        beat -= (int)math.floor(beat);
        beat += curBeat;

        float diff = beat - (float)checkBeat;

        //Debug.Log(diff);
        if (Math.Abs(diff) <= tolerance || (Math.Abs(diff) >= 4f-tolerance && Math.Abs(diff) <= 4f+tolerance))
        {
            return true;
        }

        return false;
    }
}