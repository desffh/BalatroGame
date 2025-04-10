using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrollJoker : IsStrightPlush, IPokerJoker
{
    public void PokerJoker(List<Card> cards, int saveMultiple)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        if (cards.Count == 5)
        {
            if (isFlush(cards))
            {
                Debug.Log("조커 플러쉬 발동");

                saveMultiple = 10;
            }
        }
        return;
    }
}
