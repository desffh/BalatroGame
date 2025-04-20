using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Triple : IPokerHandle
{
    public string pokerName => "트리플";
    public int plus => 30;
    public int multiple => 3;
    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);
    
        //트리플 처리
            if (cardCount.Values.Contains(3))
            {
            //Debug.Log("트리플");

            // 3과 똑같은 벨류값을 가진애 찾기
            foreach (var item in cardCount.Where(x => x.Value == 3))
                {
                    for (int i = 0; i < 3; i++)
                    {
                        saveNum.Add(item.Key);
                    }
                }
            }
        return;

    }
}
