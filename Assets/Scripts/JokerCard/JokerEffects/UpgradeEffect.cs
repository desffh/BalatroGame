using System;
using System.Collections;
using System.Collections.Generic;
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

    public bool ApplyEffect(JokerEffectContext context)
    {
        var stateManager = context.StateManager;

        var myJoker = context.MyJoker;

        var jokerCategory = context.CurrentHandType;

        // 족보 타입이 스트레이트면 실행
        if (string.Equals(jokerCategory, targetType, StringComparison.OrdinalIgnoreCase))
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

        // 족보가 스트레이트 아니면 false로 탈출
        return false;
    }
}
