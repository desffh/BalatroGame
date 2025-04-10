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
        Debug.Log($"{newCard.name} ��Ŀ�� �� ��Ŀ ����Ʈ�� �߰��߽��ϴ�!");
    }

}
