using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class HomeHp : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public GameObject gauge;
    public TextMeshProUGUI hpText;
    
    void Start()
    {
        GameManager.inst.curStage.homeHp = this;
        hp = maxHp;
    }

    public void Damaged(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
        }

        hpText.text = $"{hp} / {maxHp}";
        gauge.transform.DOScaleY(hp / (float)maxHp, 0.5f);
    }
}
