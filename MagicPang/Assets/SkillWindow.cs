using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
public class SkillWindow : MonoBehaviour
{
    public bool isOpen = false;
    
    void Start()
    {
        
    }

    
    public void ToggleWindow()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            transform.DOMoveY(-350f, 0.5f).SetEase(Ease.InOutBack,1);
        }
        else
        {
            transform.DOMoveY(-1500f, 0.5f).SetEase(Ease.InOutBack, 1);
        }
    }

    public void Punch()
    {
        
    }
    
    public void Heal()
    {
        
    }
    
    public void Meteor()
    {
        
    }
    
    public void VerticalBeam()
    {
        
    }
    
    public void HorizontalPunch()
    {
        
    }
}
