using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeEffect : IJokerEffect
{
    private string targetType;   // 족보
    private int bonus; // 배수
    private string category; // 타입

    public UpgradeEffect(string type, int bonus, string category)
    {
        targetType = type;
        this.bonus = bonus;
        this.category = category;
    }

    // 족보와 조커의 요구 족보가 같다면
    public bool ApplyEffect(JokerEffectContext context)
    {
        JokerCard myJoker = context.MyJoker;

        StateManager stateManager = context.StateManager;


        if (context.HandTypes == null) return false;

        // 현재 족보들 중 하나라도 targetType과 일치하면 발동
        if (context.HandTypes.Any(type => string.Equals(type, targetType, StringComparison.OrdinalIgnoreCase)))
        {
            // 스트레이트가 나올 때 마다 15칩 추가 
            myJoker.data.multiple += 15;

            bonus = myJoker.data.multiple;

            Debug.Log("현재 조커의 누적 칩 : " + bonus);

            stateManager.multiplyChipSetting.AddPlus(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingChip(bonus);

            return true;
        }

        return false;
    }
}
