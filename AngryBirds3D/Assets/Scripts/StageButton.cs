using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour
{
    public SceneAsset stageScene;
    public List<GameObject> stars;
    
    private void Start()
    {
        StageData data = GameManager.instance.gameData.GetStageData(stageScene.name);
        for (int i = 0; i < data.star; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void OpenStage()
    {
        GameManager.instance.OpenScene(stageScene.name);
    }

}
