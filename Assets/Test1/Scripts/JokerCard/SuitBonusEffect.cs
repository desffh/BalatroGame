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

    public bool ApplyEffect(List<Card> selectedCards, string currentHandType, StateManager stateManager, string jokerCategory, JokerCard myJoker)
    {
        var matchedCard = selectedCards.Where(card => card.itemdata.suit == targetSuit).ToList();

        if (matchedCard.Count > 0)
        {
            stateManager.MultipleChip.PlusMultiple(bonus);

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            ServiceLocator.Get<IAudioService>().PlaySFX("Sound-CheckCard");

            // ����� ��ġ�ϴ� ī����� �ִϸ��̼� ����
            foreach (var card in matchedCard)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[��Ŀ: {targetSuit}] ���� ��ġ �� �ִϸ��̼� ���: {card.name}");
            }

            TextManager.Instance.UpdateText(2, stateManager.MultipleChip.MULTIPLYSum);

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

            return true;
        }

        // �׷��� �ʴٸ� ���� ȿ������ �ִϸ��̼� ����
        return false;
    }
}

