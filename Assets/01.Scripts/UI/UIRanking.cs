using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIRanking : UIBase
{
    private const string PREFIX_RANKING = "<size=50>Ranking</size>\n";
    [SerializeField] private TextMeshProUGUI rankingText;

    public override void Init()
    {
        rankingText.text = PREFIX_RANKING;
    }

    public void UpdateRankingText(List<string> playerList)
    {
        rankingText.text = PREFIX_RANKING + GetRankingList(playerList);
    }

    private string GetRankingList(List<string> playerList)
    {
        // Get the ranking list from the game manager or relevant source
        return string.Join("\n", playerList.Select((player, index) => $"{ToOrdinal(index + 1)}. {player}"));
    }

    public string ToOrdinal(int number)
    {
        // 마지막 두 자리 확인 (11, 12, 13 예외 처리)
        int lastTwoDigits = number % 100;

        if (lastTwoDigits == 11 || lastTwoDigits == 12 || lastTwoDigits == 13)
        {
            return number + "th";
        }

        // 마지막 한 자리에 따른 규칙
        int lastDigit = number % 10;

        switch (lastDigit)
        {
            case 1:
                return number + "st";
            case 2:
                return number + "nd";
            case 3:
                return number + "rd";
            default:
                return number + "th";
        }
    }
}
