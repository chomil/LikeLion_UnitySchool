using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;

public class FloatingText : Effect
{
    public TextMeshPro floatingText;

    private void SetText(string text,float size,Color color, float yValue, float duration)
    {
        floatingText.text = text;
        floatingText.fontSize = size;
        floatingText.color = color;
        transform.DOLocalMoveY(transform.localPosition.y+yValue,duration).OnComplete(()=>{
            DestroyEffect();
        });
    }

    public void SetTextByPreset(string preset, int value = 0)
    {
        if (preset == "Good")
        {
            SetText("Good", 400, Color.white, 100, 0.5f);
        }
        else if (preset == "Miss")
        {
            SetText("Miss", 300, Color.white, 100, 0.5f);
        }
        else if (preset == "Damage")
        {
            SetText(value.ToString(), 6, Color.red, 2f, 0.5f);
        }
        else
        {
            Debug.Log("Invalid preset");
        }
    }
}
