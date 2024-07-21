using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public int maxHp = 50;
    public List<int> elementalLevel = new List<int>();

    public GameData()
    {
        for (int i = 0; i < 5; i++)
        {
            elementalLevel.Add(1);
        }
    }
}
