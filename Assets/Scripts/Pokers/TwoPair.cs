using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Diagnostics;

public class TwoPair : IPokerHandle
{
    public string pokerName => "�� ���";
    public int plus => 20;
    public int multiple => 2;


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        // �� ��� ����: ���� 2�� ���� 2�� �־�� ��
        if (cardCount.Values.Count(v => v == 2) == 2)
        {
            // ����� ���� 2�� ����
            List<int> twoPairNumbers = cardCount.Where(x => x.Value == 2).Select(x => x.Key).ToList();

            // �ش� ���ڿ� �ش��ϴ� ī�� 4�� ���͸�
            List<Card> matchedCards = cards.Where(card => twoPairNumbers.Contains(card.itemdata.id)).ToList();

            // ����� ���� �� �� ����
            foreach (var card in matchedCards)
            {
                saveNum.Add(card.itemdata.id);
            }
        }
    }

}
