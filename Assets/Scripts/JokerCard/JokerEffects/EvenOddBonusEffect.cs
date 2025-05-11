using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EvenOddBonusEffect : IJokerEffect
{
    private string targetSuit;  
    private int bonus;
    private string category; // Ȧ�� / ¦��

    public EvenOddBonusEffect(string suit, int bonus, string category)
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


        bool anyOdd = selectedCards.Any(card => card.itemdata.id % 2 != 0); // �ϳ��� Ȧ���� true

        if (category == "Odd" && !anyOdd) // Ȧ�� ��Ŀ�ε� Ȧ���� �ϳ��� ������
            return false; // �������� ����

        bool anyEven = selectedCards.Any(card => card.itemdata.id % 2 == 0);

        if (category == "Even" && !anyEven)
            return false;

        bool scoreApplied = false;

        foreach (var card in selectedCards)
        {
            if (category == "Odd" && card.itemdata.id % 2 != 0)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[��Ŀ: {targetSuit}] Ȧ�� ī�� �ִϸ��̼�: {card.name}");

                if (!scoreApplied)
                {
                    AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);
                    ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

                    stateManager.multiplyChipSetting.AddPlus(bonus);
                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingChip(myJoker.currentData.baseData.multiple);

                    scoreApplied = true;
                }
            }
            else if (category == "Even" && card.itemdata.id % 2 == 0)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[��Ŀ: {targetSuit}] ¦�� ī�� �ִϸ��̼�: {card.name}");

                if (!scoreApplied)
                {
                    AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);
                    ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

                    stateManager.multiplyChipSetting.AddMultiply(bonus);
                    myJoker.GetComponent<ShowJokerRankText>()?.OnSettingRank(myJoker.currentData.baseData.multiple);

                    scoreApplied = true;
                }
            }
        }


        return true;
    }
}
