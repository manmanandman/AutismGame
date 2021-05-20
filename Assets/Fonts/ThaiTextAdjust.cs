using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThaiTextAdjust : MonoBehaviour
{
    Text text;
    TextMeshProUGUI TextPro;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            text = this.GetComponent<Text>();
            text.text = ThaiFontAdjuster.Adjust(text.text);
        }
        catch
        {
            TextPro = this.GetComponent<TMPro.TextMeshProUGUI>();
            TextPro.text = ThaiFontAdjuster.Adjust(TextPro.text);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
