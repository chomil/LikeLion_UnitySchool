using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public List<StageData> stageDatas= new List<StageData>();

    public StageData GetStageData(string _stageName)
    {
        foreach (var data in stageDatas)
        {
            if (data.stageName == _stageName)
            {
                return data;
            }
        }
        return new StageData(_stageName);
    }

    public void SetStageData(StageData stageData)
    {
        for (int i = 0; i < stageDatas.Count; i++)
        {
            if (stageDatas[i].stageName == stageData.stageName)
            {
                stageDatas[i] = stageData;
                return;
            }
        }
        stageDatas.Add(stageData);
    }
}

[System.Serializable]
public class StageData
{
    public string stageName;
    public int highScore = 0;
    public int star = 0;
    public bool isCleared = false;

    public StageData(string name)
    {
        stageName = name;
    }
}
