using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;
    public AudioSource sfxAudioSource; 
    public AudioSource bgmAudioSource; 

   
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
            sfxAudioSource.PlayOneShot(clip, volume);
        }
    }
    
    public void PlaySound(List<AudioClip> clips, float volume = 1f)
    {
        if (clips.Count > 0)
        {
            int randomIndex = Random.Range(0, clips.Count);
            sfxAudioSource.PlayOneShot(clips[randomIndex],volume);
        }
    }

    public void PlayBGM(AudioClip clip, float volume = 1f)
    {
        if (bgmAudioSource.clip == clip)
        {
            bgmAudioSource.volume = volume;
        }
        else
        {
            bgmAudioSource.clip = clip;
            bgmAudioSource.volume = volume;
            bgmAudioSource.Play();
        }
    }
    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void SetBgmVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }
    public void SetSfxVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }
}
