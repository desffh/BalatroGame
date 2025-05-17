using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class HighCard : IPokerHandle
{
    public string pokerName => "���� ī��";

    // �⺻ ������ 
    private int basePlus = 5;
    private int baseMultiple = 1;

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
        if (cards.Count == 0)
            return;

        // ī�� �߿��� ���� ���ڰ� ū ī�� ����
        var highCard = cards
            .OrderByDescending(c => c.itemdata.id)
            .FirstOrDefault();

        if (highCard != null)
        {
            saveNum.Add(highCard.itemdata.id);
        }
    }
    public void ResetUpgrade()
    {
        upgradePlus = 0;
        upgradeMultiple = 0;
    }
}
