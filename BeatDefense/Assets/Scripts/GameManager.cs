using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    public GameData gameData;

    public AudioClip titleBgm;

    public StageManager curStage;

    public List<Character> characterPrefabs;

    public TextMeshProUGUI coinText;
    public int coin = 0;

    private void Awake()
    {
        if (inst == null)
        {
            inst = this;
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
        SoundManager.inst.PlayBGM(titleBgm,0.1f);
        coinText.text = coin.ToString();
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    public void AddCoin(int addCoin)
    {
        coin += addCoin;
        coinText.text = coin.ToString();
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

            Time.timeScale = gameData.gameSpeed;
        }
        else
        {
            Debug.Log("게임 데이터가 없습니다: " + Application.persistentDataPath);
            gameData = new GameData();
        }
    }

    public void ChangeScene(string name)
    {
        if (name == "TitleScene")
        {
            SoundManager.inst.PlayBGM(titleBgm,0.1f);
        }
        SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}