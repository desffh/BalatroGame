using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class HighCard : IPokerHandle
{
    public string pokerName => "���� ī��";
    public int plus =>  5;
    public int multiple => 1;


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

}
