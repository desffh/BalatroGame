using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultipleChipEffect : IJokerEffect
{
    private string targetSuit; // ����
    private int bonus; // ��� 
    private string category; // Ÿ��

    public MultipleChipEffect(string suit, int bonus, string category)
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

        // 10�̳� 4�� �ϳ��� ������ false
        bool noTenFour = selectedCards.Any(card => card.itemdata.id == 10 || card.itemdata.id == 4);


        // ��Ű��Ű ��Ŀ�ε� ī�忡 10�̳� 4�� �ϳ��� ���� ��� 
        if (myJoker.data.name == "��Ű��Ű" && !noTenFour)
        {
            return false;
        }

        // 14�� �ϳ��� ������ false
        bool noAce = selectedCards.Any(card => card.itemdata.id == 14);

        // ���� ��Ŀ�ε� ���̽�(14)�� �ϳ��� ���� ���
        if (myJoker.data.name == "���� ��Ŀ" && !noAce)
        {
            return false;
        }

        AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");


        bool scoreApplied = false;

        foreach (var card in selectedCards)
        {
            if (myJoker.data.name == "��Ű��Ű" && card.itemdata.id == 10 || card.itemdata.id == 4)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);

                int chip1 = 10;

                if(!scoreApplied)
                {
                    stateManager.multiplyChipSetting.AddMultiply(bonus); // 4���
                    stateManager.multiplyChipSetting.AddPlus(chip1); // 10Ĩ

                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingMultiplyChip( chip1, myJoker.data.multiple);

                    scoreApplied = true;
                }
            }

            else if (myJoker.data.name == "���� ��Ŀ" && card.itemdata.id == 14)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);

                int chip2 = 20;
                
                if(!scoreApplied)
                {
                    stateManager.multiplyChipSetting.AddMultiply(bonus); // 4���
                    stateManager.multiplyChipSetting.AddPlus(chip2); // 20Ĩ

                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingMultiplyChip(chip2, myJoker.data.multiple);

                    scoreApplied = true;
                }
            }

        }
        return true;
    }
}
