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


    private void Start()
    {
        GameManager.instance.curStage = this;
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        
        if (!curBird)
        {
            if (curBirdIndex < Birds.Count)
            {
                curBird = Instantiate(Birds[curBirdIndex],birdSpawnPos,Quaternion.identity);
                curBirdIndex++;
            }
            else
            {
                cameraController.StopFollowing();
                slingShot.EraseFlyLine();
                return;
            }
        }

        if (curBird.isMoving || curBird.isDying)
        {
            cameraController.StartFollowing(curBird);
            return;
        }
        else if (curBird.isDraging)
        {
            cameraController.StopFollowing();
        }


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