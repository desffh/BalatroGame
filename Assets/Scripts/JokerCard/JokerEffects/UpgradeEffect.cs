using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeEffect : IJokerEffect
{
    private string targetType;   // ����
    private int bonus; // ���
    private string category; // Ÿ��

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

        // ���� Ÿ���� ��Ʈ����Ʈ�� ����
        if (string.Equals(jokerCategory, targetType, StringComparison.OrdinalIgnoreCase))
        {
            // ��Ʈ����Ʈ�� ���� �� ���� 15Ĩ �߰� 
            myJoker.data.multiple += 15;

            bonus = myJoker.data.multiple;  

            Debug.Log("���� ��Ŀ�� ���� Ĩ : " + bonus);

            stateManager.multiplyChipSetting.AddPlus(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingChip(bonus);

            return true;
        }

        // ������ ��Ʈ����Ʈ �ƴϸ� false�� Ż��
        return false;
    }
}
