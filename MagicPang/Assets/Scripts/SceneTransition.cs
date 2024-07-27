using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("TransitionDestroy",2);
    }

    void TransitionDestroy()
    {
        Destroy(gameObject);
    }
}
