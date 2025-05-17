using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RoyalStraightFlush : IsStrightPlush, IPokerHandle
{
    public string pokerName => "�ξ� ��Ʈ����Ʈ �÷���";
    
    // �⺻ ������ 
    private int basePlus = 200;
    private int baseMultiple = 8;

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

        if (cardCount.Count == 5)
        {
            if (isStraight(cards) && isFlush(cards))
            {
                if (cards[0].itemdata.id == 10)
                {
                    //Debug.Log("�ξ� ��Ʈ����Ʈ �÷���");

                    for (int i = 0; i < cards.Count; i++)
                    {

                        saveNum.Add(cards[i].itemdata.id);
                    }
                }
            }
        }
        return;

    }
    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
