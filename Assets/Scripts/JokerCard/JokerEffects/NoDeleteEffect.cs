using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NoDeleteEffect : IJokerEffect
{
    private string targetSuit; // ����
    private int bonus; // ��� 
    private string category; // Ÿ��

    public NoDeleteEffect(string suit, int bonus, string category)
    {
        targetSuit = suit;
        this.bonus = bonus;
        this.category = category;
    }

    public bool ApplyEffect(JokerEffectContext context)
    {
        var selectedCards = context.SelectedCards;
        var stateManager = context.StateManager;
        var myJoker = context.MyJoker;


        // �����Ŀ�ε� ���� �����Ⱑ 0�� ���
        if (myJoker.data.name == "���" && stateManager.handDeleteSetting.GetDelete() == 0)
        {
            return false;
        }

        // �ź��� ���� ��Ŀ�ε� ���� �����Ⱑ 0�� �ƴ� ���
        if (myJoker.data.name == "�ź��� ����" && stateManager.handDeleteSetting.GetDelete() != 0)
        {
            return false;
        }

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);
        
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

        if(myJoker.data.name == "���")
        {
            int currentDelete = stateManager.handDeleteSetting.GetDelete();

            stateManager.multiplyChipSetting.AddPlus(bonus * currentDelete);

            myJoker.GetComponent<ShowJokerRankText>()?.OnSettingChip(bonus * currentDelete);
        }

        else if(myJoker.data.name == "�ź��� ����")
        {
            stateManager.multiplyChipSetting.AddMultiply(bonus);

            myJoker.GetComponent<ShowJokerRankText>()?.OnSettingRank(myJoker.data.multiple);
        }

        return true;
    }
}
