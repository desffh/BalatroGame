using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartDebuff : ICardDebuff
{
    // ���� ����� : ������ ��Ʈ��� ���� ��� �� id = 0���� ó��
    public bool ApplyDebuff(Card card)
    {
        // ������ ��Ʈ��� true ��ȯ 

        return card.itemdata.suit == "��Ʈ";
    }
}
