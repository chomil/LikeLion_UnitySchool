using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ShopWindow : MonoBehaviour
{
    private bool isOpen = true;
    public TextMeshProUGUI openText;

    public void ToggleWindow()
    {
        isOpen = !isOpen;
        
        if (isOpen)
        {
           transform.DOLocalMoveY(0f, 0.5f).SetEase(Ease.InOutBack, 1);
            openText.text = "Close Shop";
        }
        else
        {
            transform.DOLocalMoveY(580f, 0.5f).SetEase(Ease.InOutBack, 1);
            openText.text = "Open Shop";
        }
    }
}