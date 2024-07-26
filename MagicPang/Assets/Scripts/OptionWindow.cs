using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider bgmSlider;
    public Slider speedSlider;

    public TextMeshProUGUI sfxText;
    public TextMeshProUGUI bgmText;
    public TextMeshProUGUI speedText;

    private void Start()
    {
        sfxSlider.value = GameManager.inst.gameData.sfxVol*100;
        bgmSlider.value = GameManager.inst.gameData.bgmVol*100;
        speedSlider.value = GameManager.inst.gameData.gameSpeed;

        SetSfxSlider();
        SetBgmSlider();
        SetSpeedSlider();
    }

    public void SetSfxSlider()
    {
        float val = sfxSlider.value * 0.01f;
        GameManager.inst.gameData.sfxVol = val;
        SoundManager.inst.SetSfxVolume(val);

        sfxText.text = sfxSlider.value.ToString();
    }
    public void SetBgmSlider()
    {
        float val = bgmSlider.value * 0.01f;
        GameManager.inst.gameData.bgmVol = val;
        SoundManager.inst.SetBgmVolume(val);
        
        bgmText.text = bgmSlider.value.ToString();
    }
    public void SetSpeedSlider()
    {
        float val = speedSlider.value;
        GameManager.inst.gameData.gameSpeed = val;
        Time.timeScale = val;

        speedText.text = $"x{speedSlider.value}";
    }
}
