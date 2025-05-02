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

    public void ApplyEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager, string jokerCategory, JokerCard myJoker)
    {
        Debug.Log("���� Ȯ�� �ҰԿ�");

        var matchedCard = selectedCards.FirstOrDefault(card => card.itemdata.suit == targetSuit);

        if (matchedCard != null)
        {
            holdManager.MultiplySum += bonus;

            AnimationManager.Instance.PlayJokerCardAnime(myJoker.gameObject);

            AnimationManager.Instance.PlayCardAnime(matchedCard.gameObject); // �� ���⿡ ����

            TextManager.Instance.UpdateText(2, holdManager.MultiplySum);

            Debug.Log($"[��Ŀ: {targetSuit}] ���� ��ġ �� ��� +{bonus}, �ִϸ��̼� ���: {matchedCard.name}");
        }

    }
}

