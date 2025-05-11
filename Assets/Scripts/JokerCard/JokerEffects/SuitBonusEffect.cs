using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuitBonusEffect : IJokerEffect
{
    private string targetSuit;  // ��: "��Ʈ"
    private int bonus;
    private string category;

    public SuitBonusEffect(string suit, int bonus, string category)
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

        var matchedCard = selectedCards.Where(card => card.itemdata.suit == targetSuit).ToList();

        if (matchedCard.Count > 0)
        {
            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

            // ����� ��ġ�ϴ� ī����� �ִϸ��̼� ����
            foreach (var card in matchedCard)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[��Ŀ: {targetSuit}] ���� ��ġ �� �ִϸ��̼� ���: {card.name}");
            }

            stateManager.multiplyChipSetting.AddMultiply(bonus);

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

            return true;
        }

        return false;
    }
}

