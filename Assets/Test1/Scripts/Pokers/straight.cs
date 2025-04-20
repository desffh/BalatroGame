using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class straight : IsStrightPlush, IPokerHandle 
{
    public string pokerName => "스트레이트";
    public int plus => 30;
    public int multiple => 4;
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
}
