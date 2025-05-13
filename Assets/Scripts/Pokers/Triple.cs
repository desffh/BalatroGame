using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Triple : IPokerHandle
{
    public string pokerName => "Ʈ����";
    public int plus => 30;
    public int multiple => 3;


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        // ���� 3���� �ִ� �׸��� �����ϴ��� Ȯ��
        if (cardCount.Values.Contains(3))
        {
            // Ʈ���� ���� ���� (��: 6�� 3����� 6)
            var tripleNumbers = cardCount
                .Where(x => x.Value == 3)
                .Select(x => x.Key)
                .ToList();

            // �ش� ���ڸ� ���� ī��� ���͸� (�� 3��)
            var matchedCards = cards
                .Where(c => tripleNumbers.Contains(c.itemdata.id))
                .ToList();


            foreach (var card in matchedCards)
            {
                saveNum.Add(card.itemdata.id);
            }
        }
    }

}
