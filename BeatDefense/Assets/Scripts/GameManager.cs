using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    public GameData gameData;

    public SerializedDictionary<string,AudioClip> bgms;
    public SerializedDictionary<string,AudioClip> sfxs;

    public StageManager curStage;

    public List<Character> characterPrefabs;
    public List<Monster> monsterPrefabs;
    public List<Effect> effectPrefabs;
    public FloatingText floatingTextPrefab;
    public RhythmNote notePrefab;

    public Canvas mainCanvas;
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
        SoundManager.inst.PlayBGM(bgms["Default"],0.5f);
        SoundManager.inst.bgmBpm = 84f;
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
            SoundManager.inst.PlayBGM(bgms["Default"],0.5f);
            SoundManager.inst.bgmBpm = 84f;
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