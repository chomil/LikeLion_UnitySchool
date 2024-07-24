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
        if (GameManager.inst.curBoard.isMoving == true)
        {
            return;
        }

        isOpen = !isOpen;
        if (GameManager.inst.curBoard.skill != Skill.None)
        {
            if (isOpen == true)
            {
                isOpen = false;
            }
        }

        if (isOpen)
        {
            transform.DOMoveY(-350f, 0.5f).SetEase(Ease.InOutBack, 1);
        }
        else
        {
            transform.DOMoveY(-1500f, 0.5f).SetEase(Ease.InOutBack, 1);
        }
    }

    public void Punch()
    {
        GameManager.inst.curBoard.skill = Skill.Punch;
        isOpen = true;
        ToggleWindow();
    }

    public void Heal()
    {
        if (GameManager.inst.gameData.coin < 10)
        {
            return;
        }

        GameManager.inst.curBoard.AddCoin(-10);
        GameManager.inst.curBoard.curPlayer.GetDamage(-10);
    }

    public void Meteor()
    {
        if (GameManager.inst.gameData.coin < 5)
        {
            return;
        }

        GameManager.inst.curBoard.AddCoin(-5);
        GameManager.inst.curBoard.skill = Skill.Meteor;

        isOpen = true;
        ToggleWindow();
    }

    public void VerticalBeam()
    {
        if (GameManager.inst.gameData.coin < 5)
        {
            return;
        }

        GameManager.inst.curBoard.AddCoin(-5);
        GameManager.inst.curBoard.skill = Skill.Vertical;


        isOpen = true;
        ToggleWindow();
    }

    public void HorizontalPunch()
    {
        if (GameManager.inst.gameData.coin < 5)
        {
            return;
        }

        GameManager.inst.curBoard.AddCoin(-5);
        GameManager.inst.curBoard.skill = Skill.Horizontal;

        isOpen = true;
        ToggleWindow();
    }
}