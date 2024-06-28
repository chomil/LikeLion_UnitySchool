using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum BirdType
{
    Red,
    BigRed,
    Bomb,
    ThreeBlues,
    OneBlue,
    Chuck
}

public class Bird : MonoBehaviour
{
    public BirdType type = BirdType.Red;
    public float shootSpeed = 30f;
    public bool isDraging = false;
    public bool isMoving = false;
    public bool isFlying = false;
    public bool isDying = false;
    private bool isSkilled = false;
    
    private int score = 10000;

    public Rigidbody birdRigid;
    public GameObject pathSprite;


    public AudioClip chargeSound;
    public AudioClip flySound;
    public List<AudioClip> hitSounds;
    public AudioClip dieSound;
    public AudioClip skillSound;
    public GameObject dieEffect;

    public GameObject birdSkill;

    public IEnumerator drawLineCoroutine;
    private IEnumerator dieCoroutine;
    private IEnumerator clearCoroutine;

    private void Awake()
    {
        birdRigid = gameObject.GetComponent<Rigidbody>();
        drawLineCoroutine = DrawLine();
        dieCoroutine = DieCoroutine();
        clearCoroutine = ClearCoroutine();
        shootSpeed = birdRigid.mass * 30f;
    }

    private void Update()
    {
        if (isDying == true)
        {
            return;
        }

        if (isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                switch (type)
                {
                    case BirdType.Bomb:
                        StartCoroutine(dieCoroutine);
                        break;
                    
                    case BirdType.Chuck:
                    case BirdType.ThreeBlues:
                        if (isFlying)
                        {
                            Skill();
                        }
                        break;
                }
            }
            else if (isFlying == true)
            {
                Vector3 moveVector = birdRigid.velocity;
                moveVector.Normalize();
                transform.right = moveVector;
            }
            else
            {
                if (birdRigid.velocity.magnitude < 0.2f && birdRigid.angularVelocity.magnitude < 0.2f)
                {
                    StartCoroutine(dieCoroutine);
                }
            }
        }
    }

    public void ResetVelocity()
    {
        transform.forward = Vector3.forward;
        birdRigid.velocity = Vector3.zero;
        birdRigid.angularVelocity = Vector3.zero;

        StopCoroutine(drawLineCoroutine);
    }

    public void Shooting(Vector3 shootDir)
    {
        ResetVelocity();
        birdRigid.AddForce(shootDir * shootSpeed, ForceMode.Impulse);
        isMoving = true;
        isFlying = true;

        StartCoroutine(drawLineCoroutine);

        SoundManager.instance.PlaySound(flySound, 0.8f);
    }

    public void OnDamage(float damage = 1f)
    {
        if (type == BirdType.Bomb)
        {
            StartCoroutine(dieCoroutine);
        }
    }

    public void Skill()
    {
        if (isSkilled)
        {
            return;
        }
        isSkilled = true;

        SoundManager.instance.PlaySound(skillSound);
        
        if (type == BirdType.Bomb)
        {
            if (birdSkill)
            {
                GameObject skill =  Instantiate(birdSkill, transform.position, Quaternion.identity);
                skill.GetComponent<Bomb>().owner = gameObject;
            }
        }
        else if (type == BirdType.ThreeBlues)
        {
            if (birdSkill)
            {
                Vector3 moveDir = birdRigid.velocity;
                for (int i = -1; i <= 1; i++)
                {
                    GameObject newObject =  Instantiate(birdSkill, transform.position, Quaternion.identity);
                    Bird newBird = newObject.GetComponent<Bird>();
                    newBird.isMoving = true;
                    newBird.isFlying = true;
                    
                    Quaternion rotation = Quaternion.Euler(0, 0, i*10f);
                    Vector3 rotatedVelocity = rotation * moveDir;
                    newBird.birdRigid.velocity = rotatedVelocity;
                    newBird.birdRigid.angularVelocity = birdRigid.angularVelocity;

                    newBird.StartCoroutine(newBird.drawLineCoroutine);
                    
                    GameManager.instance.curStage.curBird = newBird;
                }
                
                StartCoroutine(dieCoroutine);
            }
        }
        else if (type == BirdType.Chuck)
        {
            Vector3 moveDir = birdRigid.velocity;
            if (moveDir.y > 0)
            {
                moveDir.y = 0;
            }
            if (moveDir.x == 0)
            {
                moveDir.x = 1;
            }
            moveDir.Normalize();
            birdRigid.velocity = moveDir * (shootSpeed * 0.5f);
        }
    }

    IEnumerator DrawLine()
    {
        while (true)
        {
            GameManager.instance.curStage.slingShot.spheres.Add(Instantiate(pathSprite, transform.position,
                Quaternion.identity));
            yield return new WaitForSeconds(0.08f);
        }
    }

    private void KillBird()
    {
        isDraging = false;
        isFlying = false;
        isMoving = false;
        isDying = true;
        
        ResetVelocity();
        
        //콜라이더와 리지드바디 해제
        Collider collider = GetComponent<Collider>();
        if (collider)
        {
            collider.enabled = false;
        }
        if (birdRigid)
        {
            birdRigid.isKinematic = true;
        }
        //자식 오브젝트 전부 비활성화
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        //효과 및 효과음
        if (dieEffect != null)
        {
            Instantiate(dieEffect, transform.position, Quaternion.identity);
        }
        SoundManager.instance.PlaySound(dieSound, 0.8f);
    }
    
    
    IEnumerator DieCoroutine()
    {
        KillBird();
        
        //N초 후 삭제
        if (type == BirdType.Bomb)
        {
            Skill();
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        Destroy(gameObject);
    }

    public void ClearStart()
    {
        StartCoroutine(clearCoroutine);
    }

    IEnumerator ClearCoroutine()
    {
        KillBird();
        GameManager.instance.curStage.AddScore(score);
        GameManager.instance.curStage.DrawScore(transform.position, score, ScoreType.BirdScore);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision other)
    {
        StopCoroutine(drawLineCoroutine);
        if (isFlying)
        {
            isFlying = false;
            SoundManager.instance.PlaySound(hitSounds);
        }
    }
}