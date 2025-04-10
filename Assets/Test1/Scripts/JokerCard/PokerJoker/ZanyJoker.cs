using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZanyJoker : IPokerJoker
{
    public void PokerJoker(List<Card> cards, int saveMultiple)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        //Ʈ���� ó��
        if (cardCount.Values.Contains(3))
        {
            Debug.Log("��Ŀ Ʈ���� �ߵ�");

            // 3�� �Ȱ��� �������� ������ ã��
            foreach (var item in cardCount.Where(x => x.Value == 3))
            {
                saveMultiple = 12;
            }
        }
        return;
    }
}
