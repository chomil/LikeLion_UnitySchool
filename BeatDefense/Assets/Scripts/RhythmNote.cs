using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum KeyType
{
    None,
    Down,
    Drag
}

public class RhythmNote : MonoBehaviour
{
    public GameObject keyNote;
    public GameObject longNote;
    public Text keyNoteText;

    private Vector3 spawnPos;
    public Vector3 moveDir = Vector3.right;
    public float dragTime = 0f;
    public int dragTargetBeat = 0;
    public int keyBeat = 0;
    private KeyCode key = KeyCode.None;
    private KeyCode dragKey = KeyCode.None;
    public KeyType type = KeyType.None;
    public List<Character> linkedCharacter = new List<Character>();

    void Start()
    {
    }

    public void InitNote(int _keyBeat, Vector3 _moveDir)
    {
        linkedCharacter.Clear();

        type = KeyType.None;
        key = KeyCode.None;
        keyBeat = _keyBeat;
        moveDir = _moveDir;
        dragTargetBeat = 0;

        spawnPos = new Vector3((-moveDir.x * keyBeat * 250f), 0f, 0f);
        transform.localPosition = spawnPos;

        keyNote.SetActive(false);
        longNote.SetActive(false);
    }

    public void ResetNote()
    {
        InitNote(keyBeat, moveDir);
    }

    public void SetNote(KeyType _type, KeyCode _key = KeyCode.None, int _dragTargetBeat = 0)
    {
        type = _type;
        if (type != KeyType.None && _key == KeyCode.None)
        {
            _key = GetRandomKeycode();
        }
        key = _key;
        dragTargetBeat = _dragTargetBeat;

        spawnPos = new Vector3((-moveDir.x * keyBeat * 250f), 0f, 0f);
        transform.localPosition = spawnPos;
        if (type != KeyType.None)
        {
            keyNote.SetActive(true);
            keyNoteText.text = key.ToString();
            GetComponent<RectTransform>().SetAsLastSibling();

            if (type == KeyType.Drag)
            {
                longNote.SetActive(true);
                longNote.transform.localScale = new Vector3(-moveDir.x, 1, 1);
                longNote.GetComponent<RectTransform>().sizeDelta = new Vector2(dragTargetBeat * 250f, 25f);
            }
        }
        else
        {
            keyNote.SetActive(false);
            keyNoteText.text = "";
        }
    }

    public IEnumerator ChangeKey(KeyCode _key = KeyCode.None)
    {
        if (type != KeyType.None && _key == KeyCode.None)
        {
            _key = GetRandomKeycode();
        }

        GetComponent<RectTransform>().SetAsLastSibling();
        if (type != KeyType.None)
        {
            keyNote.SetActive(true);
            keyNoteText.text = _key.ToString();
        }

        yield return new WaitForSeconds(1f);
        key = _key;
    }

    public KeyCode GetRandomKeycode()
    {
        List<KeyCode> keys = new List<KeyCode>();
        keys.Add(KeyCode.W);
        keys.Add(KeyCode.A);
        keys.Add(KeyCode.S);
        keys.Add(KeyCode.D);

        foreach (RhythmNote curNote in GameManager.inst.curStage.notes)
        {
            if (curNote == this || curNote.keyNote.activeSelf==false)
            {
                continue;
            }

            KeyCode curKey = (KeyCode)Enum.Parse(typeof(KeyCode), curNote.keyNoteText.text);
            if (keys.Contains(curKey))
            {
                keys.Remove(curKey);
            }
        }

        return keys[Random.Range(0, keys.Count)];
    }


    void Update()
    {
        if (GameManager.inst.curStage.isPlaying == false)
        {
            return;
        }

        transform.localPosition = spawnPos + moveDir * (SoundManager.inst.curBeatFloat * 250f);

        if (transform.localPosition.x < 0 && moveDir.x < 0 || transform.localPosition.x > 0 && moveDir.x > 0)
        {
            transform.localPosition -= moveDir * 1000f;
        }


        if (SoundManager.inst.prevBeat != SoundManager.inst.curBeat)
        {
            if (SoundManager.inst.curBeat == keyBeat)
            {
                if (type != KeyType.None)
                {
                    StartCoroutine(ChangeKey());
                }

                if (type == KeyType.Drag)
                {
                    if (moveDir.x > 0)
                    {
                        GameManager.inst.curStage.rhythmSquare.longNoteL.sizeDelta =
                            new Vector2(dragTargetBeat * 250f, 25f);
                    }
                    else
                    {
                        GameManager.inst.curStage.rhythmSquare.longNoteR.sizeDelta =
                            new Vector2(dragTargetBeat * 250f, 25f);
                    }
                }
            }
        }


        if (Input.GetKeyDown(key))
        {
            if (type == KeyType.Down)
            {
                if (SoundManager.inst.CompareBeat(4, keyBeat))
                {
                    key = KeyCode.None;

                    foreach (Character curCharacter in linkedCharacter)
                    {
                        curCharacter.Attack();
                    }

                    Debug.Log("GetKeyDown");
                }
            }
            else if (type == KeyType.Drag)
            {
                if (SoundManager.inst.CompareBeat(4, keyBeat))
                {
                    dragKey = key;
                    key = KeyCode.None;

                    foreach (Character curCharacter in linkedCharacter)
                    {
                        curCharacter.Attack();
                    }

                    Debug.Log("Attack");
                }
            }
        }


        if (Input.GetKey(dragKey))
        {
            if (type == KeyType.Drag)
            {
                if (SoundManager.inst.CompareBeat(4, (keyBeat + dragTargetBeat + 1) % 4)) //드래그 중인 키가 1박자 이상 늦춰지면
                {
                    dragKey = KeyCode.None;
                    foreach (Character curCharacter in linkedCharacter)
                    {
                        curCharacter.CancelAttack();
                    }

                    Debug.Log("CancelAttack");
                }
            }
        }

        if (Input.GetKeyUp(dragKey))
        {
            dragKey = KeyCode.None;
            if (type == KeyType.Drag)
            {
                if (SoundManager.inst.CompareBeat(4, (keyBeat + dragTargetBeat) % 4))
                {
                    foreach (Character curCharacter in linkedCharacter)
                    {
                        curCharacter.Shoot();
                    }

                    Debug.Log("Shoot");
                }
                else //cancel attack
                {
                    foreach (Character curCharacter in linkedCharacter)
                    {
                        curCharacter.CancelAttack();
                    }

                    Debug.Log("CancelAttack");
                }
            }
        }
    }
}