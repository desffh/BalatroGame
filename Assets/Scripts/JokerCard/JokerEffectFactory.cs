using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ�� ���������� ����
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



// ���丮 ������ �����
public static class JokerEffectFactory
{
    // ��Ŀ�� type�� ���� ȿ�� ������ ����
    private static readonly Dictionary<JokerType, Func<JokerData, IJokerEffect>> effectMap =
        new Dictionary<JokerType, Func<JokerData, IJokerEffect>>
        {
            // type�� "Color"�� ���� ȿ��(SuitBonusEffect) ����
            { JokerType.Color, data => new SuitBonusEffect(data.require, data.multiple, data.type) },

            // type�� "Poker"�� ���� ȿ��(HandTypeBonusEffect) ����
            { JokerType.Poker, data => new HandTypeBonusEffect(data.require, data.multiple, data.type) },

            // �Ϲ� ��Ŀ ����
            { JokerType.Normal, data => new NormalBonusEffect(data.require, data.multiple, data.type) },

            // ������ �߰� ��Ŀ ����
            { JokerType.PlusDelete, data => new PlusDeleteEffect(data.require, data.multiple, data.type) },

            // Ĩ ��Ŀ ����
            { JokerType.PokerChip, data => new ChipBonusEffect(data.require, data.multiple, data.type) },

            // Ȧ�� ��Ŀ
            { JokerType.Odd, data => new EvenOddBonusEffect(data.require, data.multiple, data.type) },

            // ¦�� ��Ŀ
            { JokerType.Even, data => new EvenOddBonusEffect(data.require, data.multiple, data.type) },
            
            // ���� �ڸ� ��Ŀ -> ���� ī�尡 3���̶�� 
            { JokerType.Hand, data => new SelectCardEffect(data.require, data.multiple, data.type) },

            // �߻�ȭ ��Ŀ -> ��Ŀ ī�尡 �ִٸ�
            { JokerType.Jokers, data => new JokerCardEffect(data.require, data.multiple, data.type) },

            // ��� -> �����Ⱑ �ϳ��� ���ٸ�
            { JokerType.NoDelete, data => new NoDeleteEffect(data.require, data.multiple, data.type) },
            
            // �ź��� ����, ��Ű��Ű -> ���, Ĩ ���� 
            { JokerType.MultipleChip, data => new MultipleChipEffect(data.require, data.multiple, data.type) },

            // ���� -> Ĩ ������ ��Ŀ
            { JokerType.UpgradeChip, data => new UpgradeEffect(data.require, data.multiple, data.type) }



        };

    // ��ųʸ��� ����
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
 

// string -> enum ��ȯ �Լ�
public static class JokerTypeHelper
{
    // ���ڿ��� �ް� ���������� ��ȯ
    public static bool TryParse(string typeStr, out JokerType result)
    {
        return Enum.TryParse(typeStr, ignoreCase: true, out result);
    }
}

