using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int stageScore = 0;
    
    public List<Bird> Birds;
    private int curBirdIndex = 0;
    public Bird curBird = null;
    public Vector3 birdSpawnPos;

    public SlingShot slingShot;
    private CameraController cameraController;


    private void Awake()
    {
        GameManager.instance.curStage = this;
        cameraController = Camera.main.GetComponent<CameraController>();

        //인스펙터에 넣어둔대로 버드 동적 생성 및 리스트에 다시 저장 
        for (int i = 0; i < Birds.Count; i++)
        {
            Birds[i] = Instantiate(Birds[i],birdSpawnPos + new Vector3(-i*1.3f,0,0) ,Quaternion.identity).GetComponent<Bird>();
        }
    }

    private void Update()
    {
        //스테이지 다시시작 R
        if (Input.GetKey(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        
        if (!curBird)
        {
            if (curBirdIndex < Birds.Count) //현재 버드가 없다면 다음 버드 가져오기
            {
                curBird = Birds[curBirdIndex];
                curBirdIndex++;
            }
            else //모든 버드 다 사용함
            {
                cameraController.StopFollowing();
                slingShot.EraseFlyLine();
                return;
            }
        }

        //현재 버드를 카메라가 딸아갈 버드로 설정
        cameraController.followingBird = curBird;
        
        //현재 버드가 움직이는 중이면 카메라가 따라가기
        if (curBird.isMoving || curBird.isDying)
        {
            cameraController.StartFollowing(curBird);
        }
        else
        {
            cameraController.StopFollowing();
        }

        //카메라가 안움직일때만 입력 받기
        if (cameraController.isFollowing == false && cameraController.isDraging == false)
        {
            if (Input.GetMouseButton(0))
            {
                slingShot.SetState(SlingShotState.Charge);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                slingShot.SetState(SlingShotState.Shoot);
            }
        }
    }
}