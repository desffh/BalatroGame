using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 팩토리 패턴
//
//
// string을 키 값으로 IPopupText가 붙은 게임 오브젝트(벨류값)를 반환한다

public static class JokerPopupTextFactory
{
    // 팝업 생성자 딕셔너리 (타입 → 생성 함수)
    private static readonly Dictionary<string, Func<GameObject, IPopupText>> popupMap =
        new Dictionary<string, Func<GameObject, IPopupText>>(StringComparer.OrdinalIgnoreCase)
        {
            // 타입 문자열에 따라 컴포넌트를 동적으로 Add -> 팝업 컴포넌트들은 모두 MonoBehaviour를 상속받는 상태
            { "Normal", go => go.AddComponent<NormalJokerPopup>() },
            { "Color",  go => go.AddComponent<ColorJokerPopup>() },
            { "Poker",  go => go.AddComponent<ColorJokerPopup>() },
            { "PokerChip", go => go.AddComponent<ChipJokerPopup>() },
            { "PlusDelete", go => go.AddComponent<DeletePopup>() },
            { "Odd", go => go.AddComponent<EvenOddJokerPopup>() },
            { "Even", go => go.AddComponent<EvenOddJokerPopup>() },
            { "Hand", go => go.AddComponent<SelectCardJokerPopup>() },
            { "Jokers", go => go.AddComponent<JokerCardJokerPopup>() },
            { "NoDelete", go => go.AddComponent<NoDeleteJokerPopup>() },
            { "MultipleChip", go => go.AddComponent<MultipleChipPopup>() },
            { "UpgradeChip", go => go.AddComponent<UpgradeJokerPopup>() }





        };

    // 팝업 생성 메서드
    public static IPopupText Create(string type, GameObject target)
    {
        if (popupMap.TryGetValue(type, out var creator))
            return creator.Invoke(target);

        Debug.LogWarning($"[PopupTextFactory] 지원하지 않는 타입: {type}");
        return null;
    }
}