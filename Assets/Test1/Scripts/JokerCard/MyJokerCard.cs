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

    private void OnEnable()
    {
        StageButton.OnRoundStart += ResetAllJokerEffects;
    }

    private void OnDisable()
    {
        StageButton.OnRoundStart -= ResetAllJokerEffects;
    }

    // |------------------------------------

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
            TextManager.Instance.UpdateJokerCards(myCards.Count);
        }
    }

    public void ResetJokerCard()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            Destroy(myCards[i].gameObject);
        }

        myCards.Clear();
        TextManager.Instance.UpdateJokerCards(myCards.Count);
    }

    // ��Ŀ ȿ�� ����
    private void ResetAllJokerEffects()
    {
        foreach (var card in myCards)
        {
            IJokerEffect effect = card.GetEffect(); // �������̽� �������� 
        
            if (effect is IResettableEffect resettable) // ���� �������̽��� �ִ� ��Ŀ��
            {
                resettable.ResetEffect();
            }
        }
    }
}
