using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float followSpeed = 2.0f; 
    private float returnSpeed = 5.0f;

    private Vector3 originPos;
    public bool isFollowing = false; 
    public Bird followingBird; 
    public bool isDraging = false; 
    
    private Vector3 mouseStartPos;
    private Vector3 mousePos;
    private Vector3 mouseDelta;
    
    
    void Awake()
    {
        originPos = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mouseStartPos = Input.mousePosition;
            isDraging = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDraging = false;
        }

        if (followingBird.isDraging)
        {
            isDraging = false;
        }
    }

    
    void FixedUpdate()
    {
        if (isFollowing && followingBird)
        {
            Vector3 targetPosition = new Vector3(followingBird.transform.position.x, transform.position.y, transform.position.z);
            targetPosition.x -= 20f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if(isDraging)
            {
                if (mouseStartPos == Vector3.zero)
                {
                    mouseStartPos = Input.mousePosition;
                }
                mousePos = Input.mousePosition;
                mouseDelta = mouseStartPos - mousePos;
                mouseDelta.y = 0;
                mouseDelta.z = 0;
                mouseDelta /= 10f;
                
                mouseDelta.x = Mathf.Clamp(mouseDelta.x,-10f, 40f);
                transform.position = Vector3.Lerp(transform.position, originPos+mouseDelta, Time.fixedDeltaTime);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, originPos, returnSpeed * Time.fixedDeltaTime);
            }
        }
    }
    
    
    public void StartFollowing(Bird _followingBird)
    {
        followingBird = _followingBird;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }
}
