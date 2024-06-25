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

    private Rigidbody birdRigid;
    public GameObject sphere;


    public AudioClip chargeSound;
    public AudioClip flySound;
    public AudioClip hitSound;
    public AudioClip dieSound;
    public GameObject dieEffect;

    private IEnumerator drawLineCoroutine;

    private void Awake()
    {
        birdRigid = gameObject.GetComponent<Rigidbody>();
        drawLineCoroutine = DrawLine();
    }

    private void Start()
    {
        shootSpeed = birdRigid.mass * 30f;
    }

    private void Update()
    {
        if (isMoving == true && isFlying == false)
        {
            if (birdRigid.velocity.magnitude < 0.2f && birdRigid.angularVelocity.magnitude < 0.2f)
            {
                isMoving = false;
                if (dieEffect != null)
                {
                    Instantiate(dieEffect, transform.position, Quaternion.identity);
                }

                SoundManager.instance.PlaySound(dieSound, 0.8f);

                Destroy(gameObject);
            }
        }
    }

    public void ResetPosition()
    {
        transform.forward = Vector3.forward;
        birdRigid.velocity = Vector3.zero;
        birdRigid.angularVelocity = Vector3.zero;

        StopCoroutine(drawLineCoroutine);

        SoundManager.instance.PlaySound(chargeSound, 0.8f);
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


    private void OnCollisionEnter(Collision other)
    {
        StopCoroutine(drawLineCoroutine);
        isFlying = false;
    }
}