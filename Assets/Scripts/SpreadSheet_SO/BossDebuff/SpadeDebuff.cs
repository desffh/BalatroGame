using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeDebuff : ICardDebuff
{
    // ���� ����� : ������ �����̵��� ���� ��� �� id = 0���� ó��
    public bool ApplyDebuff(Card card)
    {
        // ������ �����̵��� true ��ȯ 

        return card.itemdata.suit == "�����̵�";
    }
}
