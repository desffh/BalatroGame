using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubDebuff : ICardDebuff
{
    // ���� ����� : ������ Ŭ�ι���� ���� ��� �� id = 0���� ó��
    public bool ApplyDebuff(Card card)
    {
        // ������ Ŭ�ι���� true ��ȯ 

        return card.itemdata.suit == "Ŭ�ι�";        
    }
}
