using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FourCard : IsStrightPlush, IPokerHandle
{
    public string pokerName => "�� ī��";

    // �⺻ ������ 
    private int basePlus = 60;
    private int baseMultiple = 7;

    // ���׷��̵� �� �߰�
    private int upgradePlus = 0;
    private int upgradeMultiple = 0;

    public int plus => basePlus + upgradePlus;
    public int multiple => baseMultiple + upgradeMultiple;

    // �༺ī�带 ����ϸ� �߰�
    public void ApplyUpgrade(int plusData, int multipleData)
    {
        upgradePlus += plusData;
        upgradeMultiple += multipleData;
    }


    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        var cardCount = CardCount.Hand(cards);

        // ���� 4���� �׸��� �ϳ��� ������ ��ī��
        var fourCardNumbers = cardCount
            .Where(x => x.Value == 4)
            .Select(x => x.Key)
            .ToList();

        if (fourCardNumbers.Count == 0)
            return;

        // �ش� ���ڿ� ��ġ�ϴ� ī�常 ���� (4�常)
        var matchedCards = cards
            .Where(card => fourCardNumbers.Contains(card.itemdata.id))
            .ToList();


        if (matchedCards.Count == 4)
        {

            foreach (var card in matchedCards)
            {
                saveNum.Add(card.itemdata.id);
            }
        }
        else
        {
            // ī�尡 5���̰� (4+1 ������ ���ɼ�)
            if (cards.Count == 5 && cardCount.Count == 2)
            {
                // cardCount �� Value == 4�� Ű�� ã��
                var four = cardCount.FirstOrDefault(x => x.Value == 4);
                if (four.Key != 0)
                {
                    matchedCards = cards
                        .Where(c => c.itemdata.id == four.Key)
                        .ToList();

                    foreach (var card in matchedCards)
                    {
                        saveNum.Add(card.itemdata.id);
                    }
                }
            }
        }
    }

    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
