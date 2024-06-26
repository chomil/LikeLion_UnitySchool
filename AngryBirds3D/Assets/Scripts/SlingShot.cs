using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlingShotState
{
    Idle,
    Charge,
    Shoot
}

public class SlingShot : MonoBehaviour
{
    private SlingShotState state = SlingShotState.Idle;
    public GameObject slingShotSeat;
    public Animator animator = null;


    public AudioClip stretchClip;
    public List<AudioClip> shootClips;


    private Vector3 mouseStartPos;
    private Vector3 mousePos;
    private Vector3 mouseDelta;
    private float dragMaxRange = 500f;

    private Bird curBird;
    public List<GameObject> spheres = new List<GameObject>();

    private LineRenderer lineRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        lineRenderer = GetComponent<LineRenderer>();
    }


    public void DrawLaunchLine(Vector3 startPosition, Vector3 velocity, float gravity)
    {
        int resolution = 20;
        float timeStep = 0.1f;
        Vector3[] points = new Vector3[resolution];

        for (int i = 0; i < resolution; i++)
        {
            float time = i * timeStep;
            points[i] = CalculatePositionAtTime(startPosition, velocity, gravity, time);
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    private Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 velocity, float gravity, float time)
    {
        float x = startPosition.x + velocity.x * time;
        float y = startPosition.y + velocity.y * time - 0.5f * gravity * time * time;
        float z = startPosition.z + velocity.z * time;
        return new Vector3(x, y, z);
    }

    public void SetState(SlingShotState _state)
    {
        if (_state == state)
        {
            return;
        }

        state = _state;
        switch (state)
        {
            case SlingShotState.Idle:
                animator.SetTrigger("Cancled");
                curBird.isDraging = false;
                lineRenderer.positionCount = 0;
                break;
            case SlingShotState.Charge:
                curBird = GameManager.instance.curStage.curBird;
                mouseStartPos = Input.mousePosition;
                animator.SetTrigger("Charged");
                curBird.isDraging = true;
                curBird.ResetPosition();
                SoundManager.instance.PlaySound(stretchClip, 0.3f);
                SoundManager.instance.PlaySound(curBird.chargeSound, 0.8f);
                break;
            case SlingShotState.Shoot:
                if (mouseDelta.y <= 0 && mouseDelta.x <= 0)
                {
                    animator.SetTrigger("Cancled");
                }
                else
                {
                    animator.SetTrigger("Shooted");
                    EraseFlyLine();
                    curBird.Shooting(mouseDelta);
                    SoundManager.instance.PlaySound(shootClips, 0.3f);
                }

                curBird.isDraging = false;
                lineRenderer.positionCount = 0;
                break;
        }

        curBird.birdRigid.useGravity = !curBird.isDraging;
    }

    public void EraseFlyLine()
    {
        foreach (GameObject sphere in spheres)
        {
            Destroy(sphere);
        }

        spheres.Clear();
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case SlingShotState.Idle:

                break;
            case SlingShotState.Charge:

                mousePos = Input.mousePosition;
                mouseDelta = mouseStartPos - mousePos;
                mouseDelta.z = 0;

                if (mouseDelta.magnitude > dragMaxRange)
                {
                    mouseDelta.Normalize();
                }
                else
                {
                    mouseDelta.x = Math.Clamp(mouseDelta.x, 0, dragMaxRange) / dragMaxRange;
                    mouseDelta.y = Math.Clamp(mouseDelta.y, 0, dragMaxRange) / dragMaxRange;
                }

                mouseDelta.x = Math.Clamp(mouseDelta.x, 0f, 1f);
                mouseDelta.y = Math.Clamp(mouseDelta.y, 0f, 1f);

                animator.SetFloat("DragX", mouseDelta.x);
                animator.SetFloat("DragY", mouseDelta.y);

                curBird.transform.position = slingShotSeat.transform.position;

                DrawLaunchLine(curBird.transform.position, mouseDelta * 30f, -Physics.gravity.y);

                break;
            case SlingShotState.Shoot:

                break;
        }
    }
}