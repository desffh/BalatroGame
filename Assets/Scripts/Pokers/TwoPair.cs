using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

public class TwoPair : IPokerHandle
{
    public string pokerName => "투 페어";
    public int plus => 20;
    public int multiple => 2;


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        // 투 페어 조건: 값이 2인 것이 2개 있어야 함
        if (cardCount.Values.Count(v => v == 2) == 2)
        {
            // 페어인 숫자 2개 추출
            List<int> twoPairNumbers = cardCount.Where(x => x.Value == 2).Select(x => x.Key).ToList();

            // 해당 숫자에 해당하는 카드 4장 필터링
            List<Card> matchedCards = cards.Where(card => twoPairNumbers.Contains(card.itemdata.id)).ToList();

            // 디버프 적용 후 값 저장
            foreach (var card in matchedCards)
            {
                saveNum.Add(card.itemdata.id);
            }
        }
    }

}
