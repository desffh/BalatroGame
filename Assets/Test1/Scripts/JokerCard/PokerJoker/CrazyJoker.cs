using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyJoker : IsStrightPlush, IPokerJoker
{
    public void PokerJoker(List<Card> cards, int saveMultiple)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        if (cards.Count == 5)
        {
            if (isStraight(cards))
            {
                Debug.Log("조커 스트레이트 발동");

                //스트레이트
                saveMultiple = 12;
            }
            return;
        }
    }
}
