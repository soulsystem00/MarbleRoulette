using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICountDown : UIBase
{
    [SerializeField] private TextMeshProUGUI textCountDown;

    public override void Init()
    {
        textCountDown.text = "5";
    }

    public void Init(int value)
    {
        textCountDown.text = value.ToString();
    }

    public void SetTime(int value)
    {
        textCountDown.text = value.ToString();
    }
}
