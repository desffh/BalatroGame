using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaDebuff : ICardDebuff
{
    // ���� ����� : ������ ���̾ƶ�� ���� ��� �� id = 0���� ó��
    public bool ApplyDebuff(Card card)
    {
        // ������ ���̾ƶ�� true ��ȯ 

        return card.itemdata.suit == "���̾�";
    }
}
