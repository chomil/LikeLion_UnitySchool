using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreWindow : MonoBehaviour
{
    public TextMeshProUGUI monText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI sumText;
    

    public void SetScore()
    {
        int mon = (GameManager.inst.curBoard.stageLevel - 1);
        int combo = GameManager.inst.curBoard.maxCombo;
        int coin = GameManager.inst.curBoard.coin;

        monText.text = (mon).ToString();
        comboText.text = (combo).ToString();
        coinText.text = (coin).ToString();

        sumText.text = (mon * 20 + combo * 10 + coin).ToString();
    }

    public void ReStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
