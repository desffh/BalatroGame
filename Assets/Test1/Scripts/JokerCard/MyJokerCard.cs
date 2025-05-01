using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 
public class MyJokerCard : MonoBehaviour
{
    [SerializeField] public List<JokerCard> myCards;


    public List<JokerCard> Cards
    {
        get { return myCards; }
    }

    public void Start()
    {
        myCards = new List<JokerCard>(5);
    }

    public void AddJokerCard(JokerCard newCard)
    {
        myCards.Add(newCard);
        TextManager.Instance.UpdateJokerCards(myCards.Count);
        Debug.Log($"{newCard.name} 조커를 내 조커 리스트에 추가했습니다!");
    }

    [SerializeField] Button sellButton;

    // 판매하기 버튼을 누르면 호출 될 함수
    public void RemoveJokerCard(JokerCard card)
    {
        if (Cards.Contains(card))
        {
            Cards.Remove(card);
        }
    }
}
