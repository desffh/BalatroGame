using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OddDebuff : ICardDebuff
{
    public bool ApplyDebuff(Card card)
    {
        // ���ڰ� Ȧ����� true ��ȯ 

        return card.itemdata.id % 2 != 0;
    }
}
