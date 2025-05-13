using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipBonusEffect : IJokerEffect
{
    private string targetType;   // ��: "Ʈ����"
    private int bonus; // Ĩ ���
    private string category; // ��Ŀ Ÿ��

    public ChipBonusEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    // ������ ��Ŀ�� �䱸 ������ ���ٸ�
    public bool ApplyEffect(JokerEffectContext context)
    {
        if (context.HandTypes == null) return false;

        // ���� ������ �� �ϳ��� targetType�� ��ġ�ϸ� �ߵ�
        if (context.HandTypes.Any(type => string.Equals(type, targetType, StringComparison.OrdinalIgnoreCase)))
        {
            context.StateManager.multiplyChipSetting.AddPlus(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(context.MyJoker.gameObject);

            Debug.Log($"[��Ŀ: {targetType}] ���� ��ġ �� ��� +{bonus}");

            var showText = context.MyJoker.GetComponent<ShowJokerRankText>();

            showText.OnSettingChip(context.MyJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
