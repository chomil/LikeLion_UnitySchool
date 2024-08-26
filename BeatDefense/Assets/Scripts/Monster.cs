using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Sequence = Unity.VisualScripting.Sequence;

public class Monster : Character
{
    private int roadIndex = 0;
    private int MaxHp = 3;
    public int Hp = 3;

    private float speed = 1f;

    public RectTransform HpRect; 
    

    protected override void Start()
    {
        Vector3 pos = GameManager.inst.curStage.roads[roadIndex].transform.position;
        pos.y = 1;

        transform.position = pos;
        
        Move();
    }

    public void Damaged(int damage)
    {
        if (Hp == 0)
        {
            return;
        }
        Hp -= damage;
        Hp = Hp < 0 ? 0 : Hp;
        HpRect.localScale = new Vector3((float)Hp / (float)MaxHp,1,1);
        if (Hp == 0)
        {
            anim.SetTrigger("DieTrigger");
            StartCoroutine(DieCoroutine());
        }
        else
        {
            anim.SetTrigger("HitTrigger");
        }
    }

    public IEnumerator DieCoroutine()
    {        
        Tile curTile = GameManager.inst.curStage.roads[roadIndex];
        curTile.characterOnTile = null;
        transform.DOKill(false);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    protected override void Update()
    {
        if (Hp == 0)
        {
            return;
        }
        base.Update();
        
    }

    void Move()
    {
        Tile prevTile = GameManager.inst.curStage.roads[roadIndex];
        prevTile.characterOnTile = null;
        
        roadIndex++;
        if (GameManager.inst.curStage.roads.Count <= roadIndex)
        {
            characterMesh.transform.DOKill(false);
            transform.DOKill(false);
            Destroy(gameObject);
            return;
        }
        
        Tile nextTile = GameManager.inst.curStage.roads[roadIndex];
        nextTile.characterOnTile = this;
        Vector3 pos = nextTile.transform.position;
        pos.y = 1;

        Vector3 dir = pos - transform.position;
        dir.Normalize();

        characterMesh.transform.forward = dir;
        
        transform.DOMove(pos, speed).SetEase(Ease.Linear).OnComplete(Move);
    }
    
}
