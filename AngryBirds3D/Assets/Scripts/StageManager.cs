using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class StageManager : MonoBehaviour
{
    public int stageScore = 0;
    public int stageHighScore = 0;
    public TextMeshProUGUI scoreText;
    
    public List<Bird> Birds;
    private int curBirdIndex = 0;
    public Bird curBird = null;
    public Vector3 birdSpawnPos;

    public SlingShot slingShot;
    private CameraController cameraController;

    public int pigMaxCount = 0;
    public int pigCount = 0;
    private bool isClear = false;
    private bool isEnd = false;

    public FloatingText floatingTextPrefab;
    public ResultWindow resultWindow;

    public AudioClip startSfx;
    public AudioClip stageBgm;


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

    public void AddScore(int score)
    {
        stageScore += score;
        if (stageScore > stageHighScore)
        {
            stageHighScore = stageScore;
        }
        scoreText.text = $"HighScore : {stageHighScore:N0}\nScore : {stageScore:N0}";
    }

    private void Start()
    {
        GameObject[] pigObjects = GameObject.FindGameObjectsWithTag("Pig");
        pigCount = pigObjects.Length;
        pigMaxCount = pigCount;
        
        SoundManager.instance.PlaySound(startSfx);
        SoundManager.instance.PlayBGM(stageBgm, 0.3f);

        
        string currentSceneName = SceneManager.GetActiveScene().name;
        StageData data = GameManager.instance.gameData.GetStageData(currentSceneName);
        stageHighScore = data.highScore;
        AddScore(0);
        
        CursorManager.instance.CursorToHand();
    }

    private void OnDestroy()
    {
        CursorManager.instance.CursorToDefault();
    }

    private void Update()
    {
        if (isEnd)
        {
            return;
        }
        
        //스테이지 다시시작 R
        if (Input.GetKey(KeyCode.R))
        {
            GameManager.instance.ReloadScene();
        }

        
        if (!curBird)
        {
            if (curBirdIndex < Birds.Count) //현재 버드가 없다면 다음 버드 가져오기
            {
                curBird = Birds[curBirdIndex];
                curBirdIndex++;
                
                CursorManager.instance.CursorToHand();
            }
            else //모든 버드 다 사용함
            {
                cameraController.StopFollowing();
                slingShot.EraseFlyLine();
                StartCoroutine(ResultCoroutine());
                return;
            }
        }

        //모든 돼지 처치했다면 클리어 & 남은 버드 추가점수
        if (pigCount <= 0 && curBird.isDying==false && curBird.isMoving == false)
        {
            slingShot.SetState(SlingShotState.Idle);
            
            if (isClear == false)
            {
                StartCoroutine(ClearStageCoroutine());
            }
            else
            {
                curBird.ClearStart();
            }
            return;
        }
        

        //현재 버드를 카메라가 따라갈 버드로 설정
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
                
                CursorManager.instance.CursorToDefault();
            }
        }
    }

    IEnumerator ClearStageCoroutine()
    {
        cameraController.followingBird = curBird;
        cameraController.StartFollowing(curBird);
        slingShot.EraseFlyLine();
        yield return new WaitForSeconds(2f);
        isClear = true;
    }
    
    
    IEnumerator ResultCoroutine()
    {
        yield return new WaitForSeconds(1f);
        resultWindow.gameObject.SetActive(true);
        isEnd = true;
    }

    public void DrawScore(Vector3 worldPos, int score, ScoreType type)
    {
        FloatingText newText = Instantiate(floatingTextPrefab,worldPos,Quaternion.identity);
        newText.SetText(score.ToString(),type);
    }
}