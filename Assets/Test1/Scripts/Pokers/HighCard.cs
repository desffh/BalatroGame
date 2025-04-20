using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HighCard : IPokerHandle
{
    public string pokerName => "���� ī��";
    public int plus =>  5;
    public int multiple => 1;
    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

            //���� ī�� ó��
            if (cards.Count != 0)
            {
                //Debug.Log("���� ī��");
                var lastElement = cardCount.LastOrDefault(); // ������ ���

                saveNum.Add(lastElement.Key); // ���� ū ��
            }
        return;
    }

}
