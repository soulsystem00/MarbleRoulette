using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIResult : UIBase
{
    const string PREFIX_WINNER = "Winner: ";

    [SerializeField] TextMeshProUGUI winnerText;
    public override void Init()
    {
        winnerText.text = PREFIX_WINNER;
    }

    public void SetWinner(string winnerName)
    {
        winnerText.text = PREFIX_WINNER + winnerName;
    }
}
