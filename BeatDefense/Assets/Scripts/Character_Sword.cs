using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Sword : Character
{
    private new void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            if (SoundManager.inst.CompareBeat(fullBeat, 4))
            {
            }
        }
        base.Update();
    }

}
