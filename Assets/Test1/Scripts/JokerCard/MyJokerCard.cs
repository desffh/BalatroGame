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
        Debug.Log($"{newCard.name} ��Ŀ�� �� ��Ŀ ����Ʈ�� �߰��߽��ϴ�!");
    }

    [SerializeField] Button sellButton;

    // �Ǹ��ϱ� ��ư�� ������ ȣ�� �� �Լ�
    public void RemoveJokerCard(JokerCard card)
    {
        if (Cards.Contains(card))
        {
            Cards.Remove(card);
        }
    }
}
