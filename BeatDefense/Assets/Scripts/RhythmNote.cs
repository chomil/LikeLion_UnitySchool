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
    public Text keyNoteText;

    private Vector3 spawnPos;
    public Vector3 moveDir = Vector3.right;
    public bool isDraging = false;
    public float dragTime = 0f;
    [FormerlySerializedAs("dragTargetTime")] public float dragTargetBeat = 0f;
    public int keyBeat = 0;
    private KeyCode key = KeyCode.Mouse0;
    public KeyType type = KeyType.None;
    public List<Character> linkedCharacter = new List<Character>();

    void Start()
    {
    }

    public void InitNote(int _keyBeat, Vector3 _moveDir)
    {
        type = KeyType.None;
        key = KeyCode.None;
        keyBeat = _keyBeat;
        moveDir = _moveDir;
        dragTargetBeat = 0f;
        
        spawnPos = new Vector3((-moveDir.x * keyBeat * 250f), 0f, 0f);
        transform.localPosition = spawnPos;
        
        keyNote.SetActive(false);
    }
    
    public void ResetNote()
    {
        InitNote(keyBeat, moveDir);
    }

    public void SetNote(KeyType _type, KeyCode _key = KeyCode.None, float _dragTargetBeat = 0f)
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

        if (transform.localPosition.x < 0 && moveDir.x == -1 || transform.localPosition.x > 0 && moveDir.x == 1)
        {
            transform.localPosition -= moveDir * 1000f;
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
                    isDraging = true;
                }
            }
        }

        if (Input.GetKeyUp(key))
        {
            Debug.Log("GetKeyUp");
            isDraging = false;
        }
    }
}