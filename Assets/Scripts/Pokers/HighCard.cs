using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class HighCard : IPokerHandle
{
    public string pokerName => "하이 카드";
    public int plus =>  5;
    public int multiple => 1;


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

}
