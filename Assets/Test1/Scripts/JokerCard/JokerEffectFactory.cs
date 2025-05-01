using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JokerEffectFactory
{
    // 조커의 type (예: "Poker", "Color") 에 따라 효과 생성자 지정
    private static readonly Dictionary<string, Func<JokerData, IJokerEffect>> effectMap =
        new Dictionary<string, Func<JokerData, IJokerEffect>>(StringComparer.OrdinalIgnoreCase)
        {
            // type이 "Color"면 문양 효과(SuitBonusEffect) 생성
            { "Color", data => new SuitBonusEffect(data.require, data.multiple, data.type) },

            // type이 "Poker"면 족보 효과(HandTypeBonusEffect) 생성
            { "Poker", data => new HandTypeBonusEffect(data.require, data.multiple, data.type) },

            // 필요하다면 일반 조커(Normal)도 여기에 추가 가능
            // { "Normal", data => new AlwaysBonusEffect(data.multiple, data.type) }
        };

    public static IJokerEffect Create(JokerData data)
    {
        if (effectMap.TryGetValue(data.type, out var constructor))
        {
            return constructor(data);
        }

        Debug.LogWarning($"[JokerFactory] 알 수 없는 조커 타입: {data.type}");
        return null;
    }
}
