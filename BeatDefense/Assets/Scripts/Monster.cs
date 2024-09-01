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
    private int targetRoadIndex = 0;
    private int MaxHp = 3;
    public int Hp = 3;

    public float speed = 1f;

    public Line HpLine;

    public bool isMove = false;
    public int spawnIndex = 0;

    public float freezeMultiple = 1f;
    public float freezeTime = 0f;

    protected override void Start()
    {
        Vector3 pos = GameManager.inst.curStage.roads[0].transform.position;
        pos.y = 1;

        transform.position = pos;

        StartCoroutine(MoveCoroutine( spawnIndex * 2f));
    }

    public void Damaged(int damage)
    {
        if (Hp == 0 || isMove == false)
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
    
    public void SetFreeze(float _freezeMultiple, float _freezeTime)
    {
        if (freezeMultiple > _freezeMultiple)
        {
            freezeMultiple = _freezeMultiple;
        }
        freezeTime = _freezeTime;
    }

    protected override void Update()
    {
        if (freezeTime > 0)
        {
            freezeTime -= Time.deltaTime;
            if (freezeTime <= 0)
            {
                freezeMultiple = 1f;
                freezeTime = 0f;
            }
        }
        
        if (Hp == 0)
        {
            return;
        }

        base.Update();
        
        if (isMove)
        {
            Move();
        }
    }

    public IEnumerator MoveCoroutine(float startTime)
    {
        yield return new WaitForSeconds(startTime);
        isMove = true;
    }

    void Move()
    {
        if (GameManager.inst.curStage.roads.Count <= targetRoadIndex)
        {
            characterMesh.transform.DOKill(false);
            transform.DOKill(false);
            Destroy(gameObject);
            return;
        }

        Tile targetTile = GameManager.inst.curStage.roads[targetRoadIndex];
        Vector3 targetPos = targetTile.transform.position;
        targetPos.y = 1;

        Vector3 dir = targetPos - transform.position;
        dir.Normalize();
        
        Vector3 nextPos = transform.position + dir * (speed * freezeMultiple * Time.deltaTime);
        if (dir == Vector3.zero || Vector3.Dot(dir, targetPos - nextPos) < 0)
        {
            nextPos = targetPos;
            targetRoadIndex++;
        }

        if (dir != Vector3.zero)
        {
            characterMesh.transform.forward = dir;
        }

        transform.position = nextPos;
    }

    private void OnDestroy()
    {
        GameManager.inst.curStage.diedMonsterNum++;
    }
}