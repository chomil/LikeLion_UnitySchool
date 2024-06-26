using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultWindow : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public List<GameObject> stars;
    public AudioClip starSfx = null;
    public AudioClip clearSfx = null;
    public AudioClip failedSfx = null;
    public int starNum = 0;


    private void OnEnable()
    {
        StageManager curStage = GameManager.instance.curStage;
        int pigCount = curStage.pigCount;
        int pigMaxCount = curStage.pigMaxCount;
        int stageScore = curStage.stageScore;
        
        if (pigCount <= 0)
        {
            starNum = 1;
            if (stageScore >= pigMaxCount * 5000 + 3000)
            {
                starNum = 2;
            }
            if (stageScore >= pigMaxCount * 5000 + 10000)
            {
                starNum = 3;
            }
        }
        
        if (starNum == 0)
        {
            SoundManager.instance.PlaySound(failedSfx);
            messageText.text = "Failed";
        }
        else
        {
            SoundManager.instance.PlaySound(clearSfx);
            messageText.text = "Clear!";
        }
        StartCoroutine(StarActiveDelay());
    }

    private void OnDisable()
    {
        for (int i = 0; i < 3; i++)
        {
            stars[i].SetActive(false);
        }
    }

    private IEnumerator StarActiveDelay()
    {
        for (int i = 0; i <= starNum - 1; i++)
        {
            SoundManager.instance.PlaySound(starSfx);
            yield return new WaitForSeconds(0.5f);
            stars[i].SetActive(true);
        }
    }

}