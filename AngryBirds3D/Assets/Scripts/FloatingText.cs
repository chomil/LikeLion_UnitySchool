using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum ScoreType
{
    None,
    PigScore,
    BirdScore,
    StoneScore,
    WoodScore
}

public class FloatingText : MonoBehaviour
{
    private TextMeshPro floatingText;


    private void Awake()
    {
        floatingText = GetComponent<TextMeshPro>();
    }

    public void SetText(string text, ScoreType type)
    {
        floatingText.text = text;

        Color color = Color.black;
        switch (type)
        {
            case ScoreType.PigScore:
                color = new Color(0.1f, 0.5f, 0.1f, 1f);
                floatingText.fontSize = 10f;
                break;
            
            case ScoreType.BirdScore:
                color = new Color(0.7f, 0.1f, 0.1f, 1f);
                floatingText.fontSize = 12f;
                break;
            
            case ScoreType.StoneScore:
                color = new Color(0.1f, 0.1f, 0.1f, 1f);
                floatingText.fontSize = 9f;
                break;
            
            case ScoreType.WoodScore:
                color = new Color(0.3f, 0.15f, 0.15f, 1f);
                floatingText.fontSize = 8f;
                break;
        }

        floatingText.color  = color;
    }

    public void Remove() //애니메이션에서 호출
    {
        Destroy(gameObject);
    }
}
