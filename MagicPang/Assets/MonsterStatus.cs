using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStatus : MonoBehaviour
{
    public GameObject hpBar;
    public GameObject hpBarDelay;
    
    public TextMeshProUGUI hpText;
    public Image elementalImage;
    
    public TextMeshProUGUI lvText;

    private void Start()
    {
        
        hpBar.transform.localScale = Vector3.one;
        hpBarDelay.transform.localScale = Vector3.one;
    }
    
    
    public void UpdateHp(int hp, int maxHp)
    {
        hpBar.transform.localScale = new Vector3((float)hp / (float)maxHp, 1, 1);
        hpBarDelay.transform.DOScale(hpBar.transform.localScale, 0.5f);

        hpText.text = $"HP  {hp}/{maxHp}";
    }

}
