using System;
using System.Collections;
using System.Collections.Generic;
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

    public bool ApplyEffect(JokerEffectContext context)
    {
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;
        var jokerCategory = context.CurrentHandType;

        if (string.Equals(jokerCategory, targetType, StringComparison.OrdinalIgnoreCase))
        {

            stateManager.multiplyChipSetting.AddPlus(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            Debug.Log($"[조커: {targetType}] 족보 일치 → 칩 +{bonus}");

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingChip(myJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
