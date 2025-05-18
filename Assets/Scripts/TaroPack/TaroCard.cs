using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaroEffect
{
    public string requireSuit; // 문양
    public int requireNumber;  // 랜덤 1 ~ 14
    public int chipBonus = 30; // 칩 수

    public bool IsMatch(Card card)
    {
        return card.itemdata.suit == requireSuit && card.itemdata.id == requireNumber;
    }
}


public class TaroCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    [SerializeField] private TaroTotalData taroData;

    [SerializeField] private CanvasGroup canvasGroup;


    public event Action<TaroCard> OnCardSelected; // 카드가 클릭되면 호출되는 이벤트

    public event Action<TaroCard> OnSelectButtonClick; // 선택하기 버튼 클릭 시 호출되는 이벤트 



    public int RandomNumber { get; private set; }


    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }

    // 데이터 주입
    public void SetData(TaroTotalData data)
    {
        taroData = data;

        RandomNumber = UnityEngine.Random.Range(2, 15);

        if (cardImage != null && data != null)
        {
            cardImage.sprite = data.image;
        }
    }


    // 사용하기 버튼 클릭 시
    public void OnButtonClick()
    {
        if (taroData == null)
        {
            Debug.Log("타로 카드 데이터 없음");
            return;
        }

        // 0. 카드 상호작용 끄기
        OffRaycast();

        gameObject.GetComponent<RunPopUp>().OnExit();


        // 2. 능력 적용
        PokerManager.Instance.AddTaroEffect(taroData, RandomNumber);

        // 3. 텍스트 띄우기
        OnSelectButtonClick.Invoke(this);

        // 4. 카드 비활성화
        gameObject.SetActive(false);


    }

    // 랜덤 숫자 반환
    public int GetRandomNumber()
    {
        return RandomNumber;
    }

    // 카드 클릭 시 호출 (이벤트 트리거)
    public void OnClickCard()
    {
        OnCardSelected?.Invoke(this);
    }


    public void OffRaycast()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnRaycast()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public TaroTotalData GetData()
    {
        return taroData;
    }


    public void ResetCard(Vector3 startPos)
    {
        transform.DOKill();
        transform.position = startPos;
        transform.localScale = Vector3.one; // 커졌던 스케일 초기화
        RandomNumber = 0;
        gameObject.SetActive(false);
    }
}
