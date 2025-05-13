using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class fullHouse : IsStrightPlush, IPokerHandle
{
    public string pokerName => "풀 하우스";
    public int plus => 40;
    public int multiple => 4;


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        if(cards.Count == 5)
        {
            //풀 하우스, 포카드 처리
            if (cardCount.Count() == 2)
            {
                //Debug.Log("풀 하우스");


                //풀 하우스 (3장, 2장 ) 모두 넣기 
                for (int i = 0; i < cards.Count; i++)
                 {
                    saveNum.Add(cards[i].itemdata.id);
                }
            }
        }
        return;
    }
}
