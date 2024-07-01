using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StageManager curStage = null;
    public AudioClip mainBgm = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            
            SoundManager.instance.PlayBGM(mainBgm, 0.3f);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (sceneName == "TitleScene")
        {
            SoundManager.instance.PlayBGM(mainBgm, 0.3f);
        }
    }
}
