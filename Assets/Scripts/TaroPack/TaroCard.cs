using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaroCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    [SerializeField] private TaroTotalData taroData;

    [SerializeField] private CanvasGroup canvasGroup;


    public event Action<TaroCard> OnCardSelected; // 카드가 클릭되면 호출되는 이벤트

    public event Action<TaroTotalData> OnSelectButtonClick; // 선택하기 버튼 클릭 시 호출되는 이벤트 

    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }

    // 데이터 주입
    public void SetData(TaroTotalData data)
    {
        taroData = data;

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

        // 3. 텍스트 띄우기
        OnSelectButtonClick.Invoke(taroData);

        // 4. 카드 비활성화
        gameObject.SetActive(false);


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



    public void ResetCard(Vector3 startPos)
    {
        transform.DOKill();
        transform.position = startPos;
        transform.localScale = Vector3.one; // 커졌던 스케일 초기화
        gameObject.SetActive(false);
    }
}
