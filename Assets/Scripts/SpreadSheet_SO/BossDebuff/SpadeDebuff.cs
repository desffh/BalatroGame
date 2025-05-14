using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpadeDebuff : ICardDebuff
{
    // 보스 디버프 : 문양이 스페이드라면 점수 계산 시 id = 0으로 처리
    public bool ApplyDebuff(Card card)
    {
        // 문양이 스페이드라면 true 반환 

        return card.itemdata.suit == "스페이드";
    }
}
