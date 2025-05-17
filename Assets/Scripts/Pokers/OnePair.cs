using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class OnePair : IPokerHandle
{
    public string pokerName => "���";

    // �⺻ ������ 
    private int basePlus = 10;
    private int baseMultiple = 2;

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
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        // ���� 2��¥�� �� ���� �����ϸ�
        if (cardCount.Values.Contains(2))
        {
            // ����� ���� ���� (���� �ϳ��� ���� ��)
            var pairNumbers = cardCount
                .Where(x => x.Value == 2)
                .Select(x => x.Key)
                .ToList();

            // ī�� �߿��� �ش� ���ڸ� ���͸�
            var matchedCards = cards
                .Where(c => pairNumbers.Contains(c.itemdata.id))
                .ToList();

            
            // ����� ���� �� ����
            foreach (var card in matchedCards)
            {
                saveNum.Add(card.itemdata.id);
            }
        }
    }

    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
