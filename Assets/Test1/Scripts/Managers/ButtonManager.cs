using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class ButtonManager : Singleton<ButtonManager>
{
    [SerializeField] Button Handbutton; // 핸드 플레이
    [SerializeField] Button Treshbutton;//     버리기

    [SerializeField] HandCardPoints HandCardPoints;



    

    // 버튼 활성화 상태 여부
    private bool isButtonActive = true;

    private void Start()
    {
        Handbutton.interactable = false;
        Treshbutton.interactable = false;
    }

    private void Update()
    {
       if(isButtonActive == true && HandDelete.Instance.Hand > 0 && PokerManager.Instance.cardData.SelectCards.Count > 0) 
       {
            Handbutton.interactable = true;
       }
       else
       {
            Handbutton.interactable = false;
       }
       if(isButtonActive == true && HandDelete.Instance.Delete > 0 && PokerManager.Instance.cardData.SelectCards.Count > 0)
        {
            Treshbutton.interactable = true;
        }
       else
        {
            Treshbutton.interactable = false;

        }
    }

    // 핸드버튼을 클릭했을 때
    public void OnHandButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        StartCoroutine(CardDeletePoint());
        StartCoroutine(CardDeleteSound());  
    }


    IEnumerator CardDeleteSound()
    {
        for (int i = 0; i < PokerManager.Instance.cardData.SelectCards.Count; i++)
        {
            SoundManager.Instance.PlayCardSpawn();
            yield return new WaitForSeconds(0.12f);
        }

    }

    IEnumerator CardDeletePoint()
    {
        // 카드 클릭 비활성화
        CardManager.Instance.TurnOffAllCardColliders();

        for (int i = 0; i < PokerManager.Instance.cardData.SelectCards.Count; i++)
        {
            // 저장된 카드의 스크립트 가져오기
            Card selectedCard = PokerManager.Instance.cardData.SelectCards[i].gameObject.GetComponent<Card>();

            // 저장된 프리팹의 위치값을 변경하기 위해 컴포넌트 가져오기
            PokerManager.Instance.cardData.SelectCards[i].gameObject.GetComponent<Transform>();

            // 위치를 HandCardPoints로 이동
            PokerManager.Instance.cardData.SelectCards[i].gameObject.transform.
                DOMove(HandCardPoints.HandCardpos[i].transform.position, 0.5f);
            // 회전 0
            PokerManager.Instance.cardData.SelectCards[i].gameObject.transform.rotation = Quaternion.identity;

            // myCards 리스트에서 해당 카드 제거 (버퍼에서 가져와서 저장하는 곳)
            if (selectedCard != null && CardManager.Instance.myCards.Contains(selectedCard))
            {
                CardManager.Instance.myCards.Remove(selectedCard);
            }

            yield return new WaitForSeconds(0.15f);
        }
        // 남은 카드들 재정렬 되기
        CardManager.Instance.SetOriginOrder();
        CardManager.Instance.CardAlignment();

        // 더하기 계산
        HoldManager.Instance.Calculation();
    }

    // 버리기 버튼을 클릭했을 때
    public void OnDeleteButtonClick()
    {
        SoundManager.Instance.ButtonClick();

        isButtonActive = false;

        // 카드 클릭 비활성화
        CardManager.Instance.TurnOffAllCardColliders();

        int cardCount = PokerManager.Instance.cardData.SelectCards.Count;
        int completeCount = 0;

        for (int i = 0; i < cardCount; i++)
        {
            Card selectedCard = PokerManager.Instance.cardData.SelectCards[i].GetComponent<Card>();

            PokerManager.Instance.cardData.SelectCards[i].transform
                .DOMove(HandCardPoints.DeleteCardpos.position, 0.5f)
                .OnComplete(() => {
                    completeCount++;

                    if (completeCount >= cardCount)
                    {
                        // 모든 카드 이동이 끝난 다음에 실행
                        AfterDeleteAnimationComplete();
                    }
                });

            PokerManager.Instance.cardData.SelectCards[i].transform
                .DORotate(new Vector3(58, 122, 71), 3);

            CardManager.Instance.OnCardUsed(selectedCard);

            if (selectedCard != null && CardManager.Instance.myCards.Contains(selectedCard))
            {
                CardManager.Instance.myCards.Remove(selectedCard);
            }

            StartCoroutine(CardDeleteSound());
        }

        HoldManager.Instance.StartDeleteCard();
    }

    private void AfterDeleteAnimationComplete()
    {
        // 남은 카드들 정렬
        CardManager.Instance.SetOriginOrder();

        // 정렬 후 애니메이션 완료됐을 때 콜라이더 다시 켜기
        CardManager.Instance.CardAlignment(() => {
            CardManager.Instance.TurnOnAllCardColliders();
        });
    }


    // 이벤트에 들어갈 함수 -> 계산이 시작되면 버튼 상호작용 비활성화
    public void ButtonActive()
    {
        Handbutton.interactable = false;
        Treshbutton.interactable = false;
        
        isButtonActive = false;
    }

    public void ButtonInactive()
    {
        isButtonActive = true;
    }


    // |-----------------------------------------

    

}
