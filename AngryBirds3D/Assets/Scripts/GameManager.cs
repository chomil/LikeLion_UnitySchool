using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public StageManager curStage = null;
    public AudioClip mainBgm = null;
    public GameData gameData;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            
            LoadGameData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SoundManager.instance.PlayBGM(mainBgm, 0.3f);
    }

    public void SaveGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        string jsonData = JsonUtility.ToJson(gameData);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("게임 데이터가 저장되었습니다: " + Application.persistentDataPath);
    }

    public void LoadGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
        if (File.Exists(filePath))
        {
            string loadedJson = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(loadedJson);
            Debug.Log("게임 데이터를 불러왔습니다: " + Application.persistentDataPath);
        }
        else
        {
            Debug.Log("게임 데이터가 없습니다: " + Application.persistentDataPath);
        }
    }

    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (sceneName == "TitleScene" || sceneName == "StageScene")
        {
            SoundManager.instance.PlayBGM(mainBgm, 0.3f);
        }
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        GameManager.instance.OpenScene(currentScene.name);
    }
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
