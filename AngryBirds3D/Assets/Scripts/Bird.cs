using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BirdType
{
    Red,
    BigRed,
    Bomb
}

public class Bird : MonoBehaviour
{
    public BirdType type = BirdType.Red;
    private float shootSpeed = 30f;
    public bool isDraging = false;
    public bool isMoving = false;
    public bool isFlying = false;
    public bool isDying = false;
    
    public int score = 0;

    private Rigidbody birdRigid;
    public GameObject sphere;


    public AudioClip chargeSound;
    public AudioClip flySound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public GameObject dieEffect;

    public GameObject birdSkill;

    private IEnumerator drawLineCoroutine;
    private IEnumerator dieCoroutine;

    private void Awake()
    {
        birdRigid = gameObject.GetComponent<Rigidbody>();
        drawLineCoroutine = DrawLine();
        dieCoroutine = DieCoroutine();
    }

    private void Start()
    {
        shootSpeed = birdRigid.mass * 30f;

        SoundManager.instance.PlaySound(chargeSound, 0.8f);
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
                }
            }
            else if (isFlying == false)
            {
                if (birdRigid.velocity.magnitude < 0.2f && birdRigid.angularVelocity.magnitude < 0.2f)
                {
                    StartCoroutine(dieCoroutine);
                }
            }
        }
    }

    public void ResetPosition()
    {
        transform.forward = Vector3.forward;
        birdRigid.velocity = Vector3.zero;
        birdRigid.angularVelocity = Vector3.zero;

        StopCoroutine(drawLineCoroutine);
    }

    public void Shooting(Vector3 shootDir)
    {
        ResetPosition();
        birdRigid.AddForce(shootDir * shootSpeed, ForceMode.Impulse);
        isMoving = true;
        isFlying = true;

        StartCoroutine(drawLineCoroutine);

        SoundManager.instance.PlaySound(flySound, 0.8f);
    }

    public void Skill()
    {
        if (type == BirdType.Bomb)
        {
            if (birdSkill)
            {
                GameObject skill =  Instantiate(birdSkill, transform.position, Quaternion.identity);
                skill.GetComponent<Bomb>().owner = gameObject;
            }
        }
    }

    IEnumerator DrawLine()
    {
        while (true)
        {
            GameManager.instance.curStage.slingShot.spheres.Add(Instantiate(sphere, transform.position,
                Quaternion.identity));
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    
    IEnumerator DieCoroutine()
    {
        isDraging = false;
        isFlying = false;
        isMoving = false;
        isDying = true;
        
        StopCoroutine(drawLineCoroutine);
        
        if (dieEffect != null)
        {
            Instantiate(dieEffect, transform.position, Quaternion.identity);
        }
        SoundManager.instance.PlaySound(dieSound, 0.8f);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

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


    private void OnCollisionEnter(Collision other)
    {
        StopCoroutine(drawLineCoroutine);
        isFlying = false;
    }
}