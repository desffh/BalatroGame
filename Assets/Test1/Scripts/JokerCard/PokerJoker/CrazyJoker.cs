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
                Debug.Log("��Ŀ ��Ʈ����Ʈ �ߵ�");

                //��Ʈ����Ʈ
                saveMultiple = 12;
            }
            return;
        }
    }
}
