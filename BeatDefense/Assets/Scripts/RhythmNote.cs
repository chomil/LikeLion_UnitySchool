using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    public bool isLongNote = false;
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

    public void SetLongNote(bool _isLongNote, int _dragTargetBeat)
    {
        isLongNote = _isLongNote;
        dragTargetBeat = _dragTargetBeat;
        if (isLongNote)
        {
            longNote.SetActive(true);
            longNote.transform.localScale = new Vector3(moveDir.x * _dragTargetBeat, 1, 1);
        }
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
        key = _key == KeyCode.None ? GetRandomKeycode() : _key;
        dragTargetBeat = _dragTargetBeat;

        spawnPos = new Vector3((-moveDir.x * keyBeat * 250f), 0f, 0f);
        transform.localPosition = spawnPos;
        if (type != KeyType.None)
        {
            keyNote.SetActive(true);
            keyNoteText.text = key.ToString();
            GetComponent<RectTransform>().SetAsLastSibling();
        }
    }

    public IEnumerator ChangeKey(KeyCode _key)
    {
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
        KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        return keys[Random.Range(0, keys.Length)];
    }


    void Update()
    {
        transform.localPosition = spawnPos + moveDir * (SoundManager.inst.curBeatFloat * 250f);

        if (transform.localPosition.x < 0 && moveDir.x < 0 || transform.localPosition.x > 0 && moveDir.x > 0)
        {
            transform.localPosition -= moveDir * 1000f;
        }

        if (isLongNote)
        {
            if (moveDir.x > 0)
            {
                if (transform.localPosition.x + moveDir.x * dragTargetBeat * 250f > 0)
                {
                    longNote.GetComponent<RectTransform>().sizeDelta = new Vector2(-transform.localPosition.x, 35f);
                }
                else
                {
                    longNote.GetComponent<RectTransform>().sizeDelta = new Vector2(dragTargetBeat * 250f, 35f);
                }
            }
            else
            {
                if (transform.localPosition.x + moveDir.x * dragTargetBeat * 250f < 0)
                {
                    longNote.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.localPosition.x, 35f);
                }
                else
                {
                    longNote.GetComponent<RectTransform>().sizeDelta = new Vector2(dragTargetBeat * 250f, 35f);
                }
            }
        }


        if (SoundManager.inst.prevBeat != SoundManager.inst.curBeat)
        {
            if (SoundManager.inst.curBeat == keyBeat)
            {
                StartCoroutine(ChangeKey(GetRandomKeycode()));
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

            if (type == KeyType.Drag)
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

        if (Input.GetKeyUp(dragKey))
        {
            dragKey = KeyCode.None;
            if (type == KeyType.Drag)
            {
                if (SoundManager.inst.CompareBeat(4, (keyBeat+dragTargetBeat)%4))
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
    }
}