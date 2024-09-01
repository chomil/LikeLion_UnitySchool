using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class RhythmSquare : MonoBehaviour
{
    public Rectangle rect;
    private Animation popAnim;
    public RectTransform longNoteR;
    public RectTransform longNoteL;
    
    void Start()
    {
        popAnim = GetComponent<Animation>();
        rect = GetComponent<Rectangle>();
        GameManager.inst.curStage.rhythmSquare = this;
    }
    void Update()
    {
        if (GameManager.inst.curStage.isPlaying == false)
        {
            rect.Width = 300f;
        }
        if (Input.anyKeyDown)
        {
            if (GameManager.inst.curStage.isPlaying)
            {
                popAnim.Play();
            }
        }

        if (longNoteR.sizeDelta.x > 0)
        {
            longNoteR.sizeDelta =
                new Vector2(longNoteR.sizeDelta.x - 250f * (SoundManager.inst.bgmBpm / 60f) * Time.deltaTime,
                    longNoteR.sizeDelta.y);
        }
        if (longNoteL.sizeDelta.x > 0)
        {
            longNoteL.sizeDelta =
                new Vector2(longNoteL.sizeDelta.x - 250f * (SoundManager.inst.bgmBpm / 60f) * Time.deltaTime,
                    longNoteL.sizeDelta.y);
        }
        
        if (GameManager.inst.curStage.isPlaying == false)
        {
            longNoteR.sizeDelta = Vector2.zero;
            longNoteL.sizeDelta = Vector2.zero;
        }
    }
}
