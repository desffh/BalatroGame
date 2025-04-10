using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FourCard : IsStrightPlush, IPokerHandle
{
    public string pokerName => "포 카드";

    public int plus => 60;
    public int multiple => 7;

    public void PokerHandle(List<Card> cards, List<int> saveNum)
    {
        Dictionary<int, int> cardCount = CardCount.Hand(cards);


        //포카드 처리

        if (cards.Count == 5)
        {
            if (cardCount.Count() == 2)
            {
                var lastElement = cardCount.LastOrDefault(); // 마지막 요소
                var firstElement = cardCount.FirstOrDefault(); // 처음 요소

                if (cardCount.Count() == 2)
                {
                    if (lastElement.Value == 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            saveNum.Add(lastElement.Key);  // 포카드
                        }

                        Debug.Log("포카드");

                    }
                    else if (firstElement.Value == 4)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            saveNum.Add(firstElement.Key);  // 포카드
                        }

                        Debug.Log("포카드");
                    }

                }
            }
        }
        else if (cardCount.Values.Contains(4))
        {
            Debug.Log("포카드");

            // 4과 똑같은 벨류값을 가진애 찾기
            foreach (var item in cardCount.Where(x => x.Value == 4))
            {
                for (int i = 0; i < 4; i++)
                {
                    saveNum.Add(item.Key);
                }
            }
        }
        return;
    }
}
