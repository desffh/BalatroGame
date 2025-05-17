using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class HighCard : IPokerHandle
{
    public string pokerName => "하이 카드";

    // 기본 데이터 
    private int basePlus = 5;
    private int baseMultiple = 1;

    // 업그레이드 시 추가
    private int upgradePlus = 0;
    private int upgradeMultiple = 0;

    public int plus => basePlus + upgradePlus;
    public int multiple => baseMultiple + upgradeMultiple;

    // 행성카드를 사용하면 추가
    public void ApplyUpgrade(int plusData, int multipleData)
    {
        upgradePlus += plusData;
        upgradeMultiple += multipleData;
    }


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        if (cards.Count == 0)
            return;

        // 카드 중에서 가장 숫자가 큰 카드 선택
        var highCard = cards
            .OrderByDescending(c => c.itemdata.id)
            .FirstOrDefault();

        if (highCard != null)
        {
            saveNum.Add(highCard.itemdata.id);
        }
    }
    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
