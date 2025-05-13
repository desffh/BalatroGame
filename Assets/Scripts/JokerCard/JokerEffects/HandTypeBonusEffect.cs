using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class HandTypeBonusEffect : IJokerEffect
{
    private string targetType;   // ����
    private int bonus; // ���
    private string category; // Ÿ��

    public HandTypeBonusEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        if (context.HandTypes == null) return false;

        // ���� ������ �� �ϳ��� targetType�� ��ġ�ϸ� �ߵ�
        if (context.HandTypes.Any(type => string.Equals(type, targetType, StringComparison.OrdinalIgnoreCase)))
        {
            context.StateManager.multiplyChipSetting.AddMultiply(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(context.MyJoker.gameObject);

            Debug.Log($"[��Ŀ: {targetType}] ���� ��ġ �� ��� +{bonus}");

            var showText = context.MyJoker.GetComponent<ShowJokerRankText>();

            showText.OnSettingRank(context.MyJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
