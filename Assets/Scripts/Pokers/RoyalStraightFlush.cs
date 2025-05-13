using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RoyalStraightFlush : IsStrightPlush, IPokerHandle
{
    public string pokerName => "로얄 스트레이트 플러시";
    public int plus => 200;
    public int multiple => 8;


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        if (cardCount.Count == 5)
        {
            if (isStraight(cards) && isFlush(cards))
            {
                if (cards[0].itemdata.id == 10)
                {
                    //Debug.Log("로얄 스트레이트 플러쉬");

                    for (int i = 0; i < cards.Count; i++)
                    {

                        saveNum.Add(cards[i].itemdata.id);
                    }
                }
            }
        }
        return;

    }
}
