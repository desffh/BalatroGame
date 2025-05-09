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
        Debug.Log($"{newCard.name} 조커를 내 조커 리스트에 추가했습니다!");
    }

    [SerializeField] Button sellButton;

    // 판매하기 버튼을 누르면 호출 될 함수
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

    // 조커 효과 리셋
    private void ResetAllJokerEffects()
    {
        foreach (var card in myCards)
        {
            IJokerEffect effect = card.GetEffect(); // 인터페이스 가져오기 
        
            if (effect is IResettableEffect resettable) // 리셋 인터페이스가 있는 조커만
            {
                resettable.ResetEffect();
            }
        }
    }
}
