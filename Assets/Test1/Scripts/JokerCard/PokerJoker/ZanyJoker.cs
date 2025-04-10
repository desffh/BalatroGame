using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZanyJoker : IPokerJoker
{
    public void PokerJoker(List<Card> cards, int saveMultiple)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        //트리플 처리
        if (cardCount.Values.Contains(3))
        {
            Debug.Log("조커 트리플 발동");

            // 3과 똑같은 벨류값을 가진애 찾기
            foreach (var item in cardCount.Where(x => x.Value == 3))
            {
                saveMultiple = 12;
            }
        }
        return;
    }
}
