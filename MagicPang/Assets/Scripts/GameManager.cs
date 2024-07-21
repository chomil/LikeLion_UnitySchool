using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;

    public List<Tile> tilePrefabs;
    public GameBoard curBoard;

    public GameData gameData;

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

    private void OnApplicationQuit()
    {
        SaveGameData();
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
}
