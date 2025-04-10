using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyJokerCard : MonoBehaviour
{
    [SerializeField] List<JokerCard> myCards;

    public List<JokerCard> Cards
    {
        get { return myCards; }
    }

    public void Start()
    {
        
    }

    public void AddJokerCard(JokerCard newCard)
    {
        myCards.Add(newCard);
        Debug.Log($"{newCard.name} 조커를 내 조커 리스트에 추가했습니다!");
    }

}
