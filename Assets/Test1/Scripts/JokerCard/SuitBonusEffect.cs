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

    public bool ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker)
    {
        var matchedCard = selectedCards.Where(card => card.itemdata.suit == targetSuit).ToList();

        if (matchedCard.Count > 0)
        {
            holdManager.MultiplySum += bonus;

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            // ����� ��ġ�ϴ� ī����� �ִϸ��̼� ����
            foreach (var card in matchedCard)
            {
                AnimationManager.Instance.PlayCardAnime(card.gameObject);
                Debug.Log($"[��Ŀ: {targetSuit}] ���� ��ġ �� �ִϸ��̼� ���: {card.name}");
            }

            TextManager.Instance.UpdateText(2, holdManager.MultiplySum);

            ShowJokerRankText showJokerRankText = myJoker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(myJoker.currentData.baseData.multiple);

            return true;
        }

        // �׷��� �ʴٸ� ���� ȿ������ �ִϸ��̼� ����
        return false;
    }
}

