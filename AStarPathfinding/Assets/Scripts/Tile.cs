using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Tile parentTile = null;
    public float G = float.MaxValue, H = float.MaxValue;

    public Vector2Int tilePosition;
    public bool isWall = false;
    bool isShowPath = false;

    public TextMeshPro text;
    SpriteRenderer spriteRenderer;

    public float F()
    {
        if (G == float.MaxValue || H == float.MaxValue || G < 0 || H < 0)
        {
            return float.MaxValue;
        }
        return G + H;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitPosition(tilePosition.x, tilePosition.y);
    }

    public void Refresh()
    {
        G = float.MaxValue;
        H = float.MaxValue;
        parentTile = null;

        if(isShowPath)
        {
            isShowPath = false;
            spriteRenderer.color = Color.white;
        }
    }

    public void OnOffWall()
    {
        isShowPath = false;
        isWall = !isWall;
        if (isWall)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void printPath()
    {
        isShowPath = true;
        spriteRenderer.color = Color.green;
    }

    public void InitPosition(int x, int y)
    {
        this.tilePosition = new Vector2Int(x,y);
        text.text = tilePosition.ToString();
    }
}
