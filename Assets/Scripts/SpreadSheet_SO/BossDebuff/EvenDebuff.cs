using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenDebuff : ICardDebuff
{
    public bool ApplyDebuff(Card card)
    {
        // 숫자가 짝수라면 true 반환 

        return card.itemdata.id % 2 == 0;
    }
}
