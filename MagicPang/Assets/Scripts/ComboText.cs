using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboText : MonoBehaviour
{
    private TextMeshProUGUI comboText;
    void Awake()
    {
        comboText = GetComponent<TextMeshProUGUI>();
        Invoke("DestroyText",2);
    }

    public void SetComboText(int comboCnt)
    {
        comboText.text = $"{comboCnt} Combo!";
        comboText.fontSize = (100f + comboCnt * 10f)>200f?200f:100f + comboCnt * 10f;
    }
    
    private void DestroyText()
    {
        Destroy(gameObject);
    }
}
