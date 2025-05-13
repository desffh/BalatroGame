using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    // ������ ��Ŀ�� �䱸 ������ ���ٸ�
    public bool ApplyEffect(JokerEffectContext context)
    {
        JokerCard myJoker = context.MyJoker;

        StateManager stateManager = context.StateManager;


        if (context.HandTypes == null) return false;

        // ���� ������ �� �ϳ��� targetType�� ��ġ�ϸ� �ߵ�
        if (context.HandTypes.Any(type => string.Equals(type, targetType, StringComparison.OrdinalIgnoreCase)))
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

        return false;
    }
}
