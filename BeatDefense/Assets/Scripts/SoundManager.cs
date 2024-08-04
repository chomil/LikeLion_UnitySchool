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
    public float bgmVol = 0.1f;
   
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
            sfxAudioSource.PlayOneShot(clip, volume* GameManager.inst.gameData.sfxVol);
        }
    }
    
    public void PlaySound(List<AudioClip> clips, float volume = 1f)
    {
        if (clips.Count > 0)
        {
            int randomIndex = Random.Range(0, clips.Count);
            sfxAudioSource.PlayOneShot(clips[randomIndex],volume * GameManager.inst.gameData.sfxVol);
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
            curVol = math.lerp(startVol, 0, time*2);
            bgmAudioSource.volume = curVol;
            yield return null;
        }
        SetBgmVolume(0);

        float targetVol = volume * GameManager.inst.gameData.bgmVol;
        curVol = 0;
        time = 0;
        bgmAudioSource.clip = nextClip;
        bgmAudioSource.Play();

        while (curVol < targetVol)
        {
            time += Time.deltaTime;
            curVol = math.lerp(0, targetVol, time*2);
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
        bgmAudioSource.volume = bgmVol*volume;
    }

    public int CountBeat(int fullBeat)
    {
        float bpm = 105;
        float bps = bpm / 60f * ((float)fullBeat/4f);
        float beat = bgmAudioSource.time * bps;

        int cnt = (int)math.floor(beat) % fullBeat + 1;
        return cnt;
    }
}