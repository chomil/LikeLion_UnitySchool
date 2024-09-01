using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Shapes;
using TMPro;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI contextText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI priceText;

    public GameObject upgradeButton;

    private bool isOpen = false;

    public void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void SetCharacter(int characterIndex)
    {
        Character character = GameManager.inst.characterPrefabs[characterIndex];
        SetCharacter(character);
    }

    public void SetCharacter(Character character)
    {
        if (character == null)
        {
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(character.transform.position);
        screenPos.z = 0;
        screenPos.x -= Screen.width / 2f;
        screenPos.y -= Screen.height / 2f;
        screenPos.y = Mathf.Clamp(screenPos.y + 50f, -220f, 380f);
        screenPos.x += character.transform.position.x < 0 ? 200f : -200f;
        transform.localPosition = screenPos;

        levelText.text = character.level.ToString();
        upgradeButton.SetActive(character.level < 3);


        rangeText.text = ((int)(character.range.GetComponent<Disc>().Radius) / 2).ToString();
        damageText.text = character.attDamage.ToString();
        priceText.text = character.upgradePrice.ToString();
        switch (character.characterType)
        {
            case CharacterType.Sword:
                characterText.text = "SwordMan";
                contextText.text = "근거리 적 1명에게 공격합니다";
                break;
            case CharacterType.Bow:
                characterText.text = "Archer";
                contextText.text = "원거리 적 1명에게 공격합니다";
                break;
            case CharacterType.Magic:
                characterText.text = "Magician";
                contextText.text = $"근거리 적 모두에게 공격하고\n1초동안 속도를 {character.level * 20}% 늦춥니다";
                break;
        }
    }

    public void OpenWindow(bool _isOpen)
    {
        if (isOpen == _isOpen)
        {
            return;
        }

        isOpen = _isOpen;

        if (isOpen)
        {
            transform.DOKill();
            transform.DOScale(1f, 0.3f).SetEase(Ease.InOutBack, 1);
        }
        else
        {
            transform.DOKill();
            transform.DOScale(0f, 0.3f).SetEase(Ease.InOutBack, 1);
        }
    }
}