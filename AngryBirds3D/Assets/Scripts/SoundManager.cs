using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource sfxAudioSource; 
    public AudioSource bgmAudioSource; 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
}
