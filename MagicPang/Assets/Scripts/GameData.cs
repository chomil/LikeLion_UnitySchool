using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int stageLevel = 1;
    public int monsterIndex = -1;
    public int maxHp = 50;
    public int hp = 50;
    public List<int> elementalLevel = new List<int>();
    public int coin = 0;

    public GameData()
    {
        for (int i = 0; i < 5; i++)
        {
            elementalLevel.Add(1);
        }
    }
}
