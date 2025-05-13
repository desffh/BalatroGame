using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChipBonusEffect : IJokerEffect
{
    private string targetType;   // 예: "트리플"
    private int bonus; // 칩 배수
    private string category; // 조커 타입

    public ChipBonusEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    // 족보와 조커의 요구 족보가 같다면
    public bool ApplyEffect(JokerEffectContext context)
    {
        if (context.HandTypes == null) return false;

        // 현재 족보들 중 하나라도 targetType과 일치하면 발동
        if (context.HandTypes.Any(type => string.Equals(type, targetType, StringComparison.OrdinalIgnoreCase)))
        {
            context.StateManager.multiplyChipSetting.AddPlus(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(context.MyJoker.gameObject);

            Debug.Log($"[조커: {targetType}] 족보 일치 → 배수 +{bonus}");

            var showText = context.MyJoker.GetComponent<ShowJokerRankText>();

            showText.OnSettingChip(context.MyJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
