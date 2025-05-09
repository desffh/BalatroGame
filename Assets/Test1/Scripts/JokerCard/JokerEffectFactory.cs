using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���丮 ������ �����
public static class JokerEffectFactory
{
    // ��Ŀ�� type (��: "Poker", "Color") �� ���� ȿ�� ������ ����
    private static readonly Dictionary<string, Func<JokerData, IJokerEffect>> effectMap =
        new Dictionary<string, Func<JokerData, IJokerEffect>>(StringComparer.OrdinalIgnoreCase)
        {
            // type�� "Color"�� ���� ȿ��(SuitBonusEffect) ����
            { "Color", data => new SuitBonusEffect(data.require, data.multiple, data.type) },

            // type�� "Poker"�� ���� ȿ��(HandTypeBonusEffect) ����
            { "Poker", data => new HandTypeBonusEffect(data.require, data.multiple, data.type) },

            // �Ϲ� ��Ŀ ����
            { "Normal", data => new NormalBonusEffect(data.require, data.multiple, data.type) },

            // ������ �߰� ��Ŀ ����
            { "PlusDelete", data => new PlusDeleteEffect(data.require, data.multiple, data.type) },

            // Ĩ ��Ŀ ����
            { "PokerChip", data => new ChipBonusEffect(data.require, data.multiple, data.type) }

        };

    public static IJokerEffect Create(JokerData data)
    {
        if (effectMap.TryGetValue(data.type, out var constructor))
        {
            return constructor(data);
        }

        Debug.LogWarning($"[JokerFactory] �� �� ���� ��Ŀ Ÿ��: {data.type}");
        return null;
    }
}
