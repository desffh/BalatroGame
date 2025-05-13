using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class HandTypeBonusEffect : IJokerEffect
{
    private string targetType;   // 족보
    private int bonus; // 배수
    private string category; // 타입

    public HandTypeBonusEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        if (context.HandTypes == null) return false;

        // 현재 족보들 중 하나라도 targetType과 일치하면 발동
        if (context.HandTypes.Any(type => string.Equals(type, targetType, StringComparison.OrdinalIgnoreCase)))
        {
            context.StateManager.multiplyChipSetting.AddMultiply(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(context.MyJoker.gameObject);

            Debug.Log($"[조커: {targetType}] 족보 일치 → 배수 +{bonus}");

            var showText = context.MyJoker.GetComponent<ShowJokerRankText>();

            showText.OnSettingRank(context.MyJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
