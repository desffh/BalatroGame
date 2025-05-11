using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighCard : IPokerHandle
{
    public string pokerName => "하이 카드";
    public int plus =>  5;
    public int multiple => 1;
    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

            //하이 카드 처리
            if (cards.Count != 0)
            {
                //Debug.Log("하이 카드");
                var lastElement = cardCount.LastOrDefault(); // 마지막 요소

                saveNum.Add(lastElement.Key); // 가장 큰 값
            }
        return;
    }

}
