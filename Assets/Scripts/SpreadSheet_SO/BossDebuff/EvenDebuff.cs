using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenDebuff : ICardDebuff
{
    public bool ApplyDebuff(Card card)
    {
        // ���ڰ� ¦����� true ��ȯ 

        return card.itemdata.id % 2 == 0;
    }
}
