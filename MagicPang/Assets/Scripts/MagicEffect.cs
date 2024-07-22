using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MagicEffect : MonoBehaviour
{
    public Elemental elemental = Elemental.None;
    private TrailRenderer trail;
    
    
    
    public void Initialize(Elemental _elemental)
    {
        elemental = _elemental;
        
        switch (elemental)
        {
            case Elemental.Fire:
                trail.startColor = new Color(0.8f, 0.2f, 0.2f, 1f);
                trail.endColor = new Color(1f, 0f, 0f, 0f);
                break;
            case Elemental.Grass:
                trail.startColor = new Color(0.2f, 0.8f, 0.2f, 1f);
                trail.endColor = new Color(0.2f, 0.8f, 0.2f, 0f);
                break;
            case Elemental.Ground:
                trail.startColor = new Color(0.8f, 0.8f, 0.2f, 1f);
                trail.endColor = new Color(0.8f, 0.8f, 0.2f, 0f);
                break;
            case Elemental.Electric:
                trail.startColor = new Color(0.4f, 0.2f, 0.6f, 1f);
                trail.endColor = new Color(0.5f, 0.2f, 0.8f, 0f);
                break;
            case Elemental.Water:
                trail.startColor = new Color(0.2f, 0.2f, 0.8f, 1f);
                trail.endColor = new Color(0.2f, 0.2f, 0.8f, 0f);
                break;
        }

        Monster targetMon = GameManager.inst.curBoard.curMonster;

        if (targetMon)
        {
            float multiple = targetMon.MultipleDamage(elemental);
            if (multiple == 0)
            {
                Color startColor = trail.startColor;
                startColor.a = 0.05f;
                trail.startColor = startColor;
            }
            else if (multiple == 3f)
            {
                trail.startWidth = 0.3f;
            }
            
            transform.DOMove(targetMon.hitPos.transform.position, 0.3f).OnComplete(()=>
            {
                targetMon.GetDamage(elemental);
                Destroy(gameObject);
            });
        }
    }
    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
