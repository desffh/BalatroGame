using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flush : IsStrightPlush, IPokerHandle
{
    public string pokerName => "플러쉬";

    public int plus => 35;
    public int multiple => 4;

    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        if (cards.Count == 5)
        {
            if (isFlush(cards))
            {
                //Debug.Log("플러쉬");
                for (int i = 0; i < cards.Count; i++)
                {
                    saveNum.Add(cards[i].itemdata.id);
                }
            }
        }
        return;
    }
}
