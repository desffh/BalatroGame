using System;
using System.Collections;
using System.Collections.Generic;
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
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;
        var jokerCategory = context.CurrentHandType;


        // string.Equals(a, b, StringComparison.OrdinalIgnoreCase) 대소문자 구분없이 문자열 구분

        if (string.Equals(jokerCategory, targetType, StringComparison.OrdinalIgnoreCase))
        {
            stateManager.multiplyChipSetting.AddMultiply(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            Debug.Log($"[조커: {targetType}] 족보 일치 → 배수 +{bonus}");
        
            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}
