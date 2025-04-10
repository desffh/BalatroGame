using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JollyJoker : IPokerJoker
{
    public void PokerJoker(List<Card> cards, int saveMultiple)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        if (cardCount.Values.Contains(2))
        {
            Debug.Log("¿øÆä¾î");
            foreach (var item in cardCount.Where(x => x.Value == 2))
            {
                saveMultiple = 8;
            }
        }
        return;
    }
}
