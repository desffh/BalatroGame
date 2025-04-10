using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedJoker : IColorJoker
{
    public void ColorJoker(List<Card> cards, List<Card> saveSuit)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].itemdata.suit == "ÇÏÆ®")
            {
                saveSuit.Add(cards[i]);
            }
        }
        return;
    }
}
