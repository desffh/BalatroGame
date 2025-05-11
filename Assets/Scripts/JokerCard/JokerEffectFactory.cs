using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타입 열거형으로 정의
public enum JokerType
{
    Color,
    Poker,
    Even,
    Odd,
    Hand,
    Jokers,
    Normal,
    PlusDelete,
    PokerChip,
    NoDelete,
    MultipleChip,
    UpgradeChip
}



// 팩토리 패턴을 사용함
public static class JokerEffectFactory
{
    // 조커의 type에 따라 효과 생성자 지정
    private static readonly Dictionary<JokerType, Func<JokerData, IJokerEffect>> effectMap =
        new Dictionary<JokerType, Func<JokerData, IJokerEffect>>
        {
            // type이 "Color"면 문양 효과(SuitBonusEffect) 생성
            { JokerType.Color, data => new SuitBonusEffect(data.require, data.multiple, data.type) },

            // type이 "Poker"면 족보 효과(HandTypeBonusEffect) 생성
            { JokerType.Poker, data => new HandTypeBonusEffect(data.require, data.multiple, data.type) },

            // 일반 조커 생성
            { JokerType.Normal, data => new NormalBonusEffect(data.require, data.multiple, data.type) },

            // 버리기 추가 조커 생성
            { JokerType.PlusDelete, data => new PlusDeleteEffect(data.require, data.multiple, data.type) },

            // 칩 조커 생성
            { JokerType.PokerChip, data => new ChipBonusEffect(data.require, data.multiple, data.type) },

            // 홀수 조커
            { JokerType.Odd, data => new EvenOddBonusEffect(data.require, data.multiple, data.type) },

            // 짝수 조커
            { JokerType.Even, data => new EvenOddBonusEffect(data.require, data.multiple, data.type) },
            
            // 반쪽 자리 조커 -> 현재 카드가 3장이라면 
            { JokerType.Hand, data => new SelectCardEffect(data.require, data.multiple, data.type) },

            // 추상화 조커 -> 조커 카드가 있다면
            { JokerType.Jokers, data => new JokerCardEffect(data.require, data.multiple, data.type) },

            // 깃발 -> 버리기가 하나도 없다면
            { JokerType.NoDelete, data => new NoDeleteEffect(data.require, data.multiple, data.type) },
            
            // 신비의 정점, 워키토키 -> 배수, 칩 증가 
            { JokerType.MultipleChip, data => new MultipleChipEffect(data.require, data.multiple, data.type) },

            // 주자 -> 칩 성장형 조커
            { JokerType.UpgradeChip, data => new UpgradeEffect(data.require, data.multiple, data.type) }



        };

    // 딕셔너리에 생성
    public static IJokerEffect Create(JokerData data)
    {
        if (!JokerTypeHelper.TryParse(data.type, out JokerType jokerType))
        {
            Debug.LogWarning($"[JokerEffectFactory] Unknown type: {data.type}");
            return null;
        }

        if (effectMap.TryGetValue(jokerType, out var constructor))
        {
            return constructor(data);
        }

        Debug.LogWarning($"[JokerEffectFactory] No effect registered for: {jokerType}");
        return null;
    }
}
 

// string -> enum 변환 함수
public static class JokerTypeHelper
{
    // 문자열을 받고 열거형으로 반환
    public static bool TryParse(string typeStr, out JokerType result)
    {
        return Enum.TryParse(typeStr, ignoreCase: true, out result);
    }
}

