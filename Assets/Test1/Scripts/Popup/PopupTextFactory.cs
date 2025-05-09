using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ���丮 ����
//
//
// string�� Ű ������ IPopupText�� ���� ���� ������Ʈ(������)�� ��ȯ�Ѵ�

public static class PopupTextFactory
{
    // �˾� ������ ��ųʸ� (Ÿ�� �� ���� �Լ�)
    private static readonly Dictionary<string, Func<GameObject, IPopupText>> popupMap =
        new Dictionary<string, Func<GameObject, IPopupText>>(StringComparer.OrdinalIgnoreCase)
        {
            // Ÿ�� ���ڿ��� ���� ������Ʈ�� �������� Add
            { "Normal", go => go.AddComponent<NormalJokerPopup>() },
            { "Color",  go => go.AddComponent<ColorJokerPopup>() },
            { "Poker",  go => go.AddComponent<ColorJokerPopup>() },
            { "PokerChip", go => go.AddComponent<ChipJokerPopup>() },
            { "PlusDelete", go => go.AddComponent<DeletePopup>() }
        };

    // �˾� ���� �޼���
    public static IPopupText Create(string type, GameObject target)
    {
        if (popupMap.TryGetValue(type, out var creator))
            return creator.Invoke(target);

        Debug.LogWarning($"[PopupTextFactory] �������� �ʴ� Ÿ��: {type}");
        return null;
    }
}