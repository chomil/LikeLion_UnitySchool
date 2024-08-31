using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Tools;
using Shapes;
using UnityEngine;
using Sequence = Unity.VisualScripting.Sequence;

public class Monster : Character
{
    private int roadIndex = 0;
    private int MaxHp = 3;
    public int Hp = 3;

    private float speed = 1f;

    public Line HpLine;

    public bool canHit = false;
    public int spawnIndex = 0;


    protected override void Start()
    {
        Vector3 pos = GameManager.inst.curStage.roads[roadIndex].transform.position;
        pos.y = 1;

        transform.position = pos;

        Invoke(nameof(Move), spawnIndex * 2f);
    }

    public void Damaged(int damage)
    {
        if (Hp == 0 || canHit == false)
        {
            return;
        }

        Hp -= damage;
        Hp = Hp < 0 ? 0 : Hp;
        HpLine.End = new Vector3((float)Hp / (float)MaxHp, 0, 0);
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
        transform.DOKill(false);
        yield return new WaitForSeconds(1f);

        Effect coinEffect =
            Instantiate(GameManager.inst.effectPrefabs[0], GameManager.inst.mainCanvas.transform); //Spawn Coin
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        screenPos.z = 0;
        screenPos.x -= Screen.width / 2f;
        screenPos.y -= Screen.height / 2f;
        coinEffect.transform.localPosition = screenPos;
        coinEffect.transform.DOLocalMove(new Vector3(-900f, -480f, 0f), 1f).SetEase(Ease.InBack,1f)
            .OnComplete(() =>
                {
                    GameManager.inst.AddCoin(10);
                    coinEffect.DestroyEffect();
                }
            );


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
        canHit = true;

        Tile prevTile = GameManager.inst.curStage.roads[roadIndex];

        roadIndex++;
        if (GameManager.inst.curStage.roads.Count <= roadIndex)
        {
            characterMesh.transform.DOKill(false);
            transform.DOKill(false);
            Destroy(gameObject);
            return;
        }

        Tile nextTile = GameManager.inst.curStage.roads[roadIndex];
        Vector3 pos = nextTile.transform.position;
        pos.y = 1;

        Vector3 dir = pos - transform.position;
        dir.Normalize();

        characterMesh.transform.forward = dir;

        transform.DOMove(pos, speed).SetEase(Ease.Linear).OnComplete(Move);
    }

    private void OnDestroy()
    {
        GameManager.inst.curStage.diedMonsterNum++;
    }
}