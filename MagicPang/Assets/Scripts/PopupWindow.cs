using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    public GameObject popupWindow;
    public AudioClip openSfx;
    public AudioClip closeSfx;
    
    public void ActiveWindow(bool isActive)
    {
        if (isActive)
        {
            gameObject.SetActive(true);
            SoundManager.inst.PlaySound(openSfx);
            popupWindow.transform.localScale = Vector3.zero;
            popupWindow.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack,1);
        }
        else
        {
            SoundManager.inst.PlaySound(closeSfx);
            popupWindow.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack,1).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
