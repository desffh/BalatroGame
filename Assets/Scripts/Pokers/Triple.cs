using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Triple : IPokerHandle
{
    public string pokerName => "Ʈ����";
    
    // �⺻ ������ 
    private int basePlus = 30;
    private int baseMultiple = 3;

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
    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
