using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float followSpeed = 2.0f; 
    private float returnSpeed = 5.0f;

    private Vector3 originPos;
    private bool isFollowing = false; 
    public Bird followingBird; 
    void Start()
    {
        originPos = transform.position;
    }

    
    void FixedUpdate()
    {
        if (isFollowing && followingBird)
        {
            Vector3 targetPosition = new Vector3(followingBird.transform.position.x, transform.position.y, transform.position.z);
            targetPosition.x -= 20f;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originPos, returnSpeed * Time.deltaTime);
        }
    }
    
    
    // 발사체를 따라가기 시작하는 메서드
    public void StartFollowing(Bird _followingBird)
    {
        followingBird = _followingBird;
        isFollowing = true;
    }

    // 발사체를 따라가기를 멈추는 메서드
    public void StopFollowing()
    {
        isFollowing = false;
    }
}
