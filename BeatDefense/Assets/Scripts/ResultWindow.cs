using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ResultWindow : MonoBehaviour
{
    private bool isOpen = false;
    public AudioClip winClip;
    public AudioClip loseClip;
    public GameObject winImage;
    public GameObject loseImage;
    public TextMeshProUGUI message;
    
    public void Start()
    {
        transform.localScale = Vector3.zero;
    }


    public void OpenWindow(bool _isOpen, bool isWin)
    {
        if (isOpen == _isOpen)
        {
            return;
        }

        isOpen = _isOpen;
        SoundManager.inst.PlaySound(GameManager.inst.sfxs["Pop"]);
        if (isOpen)
        {
            if (isWin)
            {
                message.text = "디펜스 성공!";
                winImage.SetActive(true);
                loseImage.SetActive(false);
                SoundManager.inst.PlaySound(winClip);
            }
            else
            {
                message.text = "디펜스 실패!";
                winImage.SetActive(false);
                loseImage.SetActive(true);
                SoundManager.inst.PlaySound(loseClip);
            }
            
            transform.DOScale(1f, 0.3f).SetEase(Ease.InOutBack, 1);
        }
        else
        {
            transform.DOScale(0f, 0.3f).SetEase(Ease.InOutBack, 1);
        }
    }
}
