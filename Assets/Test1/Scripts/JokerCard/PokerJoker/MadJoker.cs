using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MadJoker : IPokerJoker
{
    public void PokerJoker(List<Card> cards, int saveMultiple)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);

        // 捧其绢 贸府
        if (cardCount.Values.Count(v => v == 2) == 2)
        {
            Debug.Log("捧其绢");

            foreach (var item in cardCount.Where(x => x.Value == 2))
            {
                saveMultiple = 10;
            }
        }
        return;
    }
}
