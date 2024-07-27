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
        Invoke("DestroyText",10);
    }

    public void SetComboText(int comboCnt)
    {
        comboText.text = $"{comboCnt} Combo!";
    }
    
    private void DestroyText()
    {
        Destroy(gameObject);
    }
}
