using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;

    public Texture2D cursorDefault;
    public Texture2D cursorClick;
    public Texture2D cursorHand;
    public Texture2D cursorGrip;
    private Vector2 hotspotPos;
    public bool isCursorHand = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        hotspotPos = new Vector2(10, 0);
        Cursor.SetCursor(cursorDefault, hotspotPos, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(isCursorHand ? cursorGrip : cursorClick, hotspotPos, CursorMode.Auto);
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(isCursorHand ? cursorHand : cursorDefault, hotspotPos, CursorMode.Auto);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(isCursorHand ? cursorGrip : cursorClick, hotspotPos, CursorMode.Auto);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(isCursorHand ? cursorHand : cursorDefault, hotspotPos, CursorMode.Auto);
        }
    }

    public void CursorToHand()
    {
        isCursorHand = true;
        Cursor.SetCursor(cursorHand, hotspotPos, CursorMode.Auto);
    }

    public void CursorToDefault()
    {
        isCursorHand = false;
        Cursor.SetCursor(cursorDefault, hotspotPos, CursorMode.Auto);
    }
}