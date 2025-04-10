using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeJoker : IColorJoker
{
    public void ColorJoker(List<Card> cards, List<Card> saveSuit)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].itemdata.suit == "다이아몬드")
            {
                saveSuit.Add(cards[i]);
            }
        }
        return;
    }
}


