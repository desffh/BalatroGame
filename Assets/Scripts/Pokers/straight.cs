using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class straight : IsStrightPlush, IPokerHandle 
{
    public string pokerName => "스트레이트";

    // 기본 데이터 
    private int basePlus = 30;
    private int baseMultiple = 4;

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
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        if (cards.Count == 5)
        {
            if (isStraight(cards))
            {
                //Debug.Log("스트레이트");


                //스트레이트
                for (int i = 0; i < cards.Count; i++)
                {

                    saveNum.Add(cards[i].itemdata.id);
                }
            }
        }
            return;

    }
    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
