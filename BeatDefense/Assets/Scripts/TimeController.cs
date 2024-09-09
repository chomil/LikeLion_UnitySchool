using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float timeScale = 1;
    public TextMeshProUGUI scaleText;

    public void TimeScaleUp()
    {
        SoundManager.inst.PlaySound(GameManager.inst.sfxs["Click"]);
        
        timeScale += 0.25f;
        if (timeScale > 1.5f)
        {
            timeScale = 1f;
        }

        scaleText.text = $"x{timeScale}";
        Time.timeScale = timeScale;
        SoundManager.inst.bgmAudioSource.pitch = timeScale;
    }
}
