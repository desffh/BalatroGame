using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlanetCard : MonoBehaviour
{
    [SerializeField] private Image cardImage; // 카드에 표시될 이미지

    [SerializeField] private PlanetTotalData planetData;

    [SerializeField] private CanvasGroup canvasGroup;

    
    public event Action<PlanetCard> OnCardSelected; // 카드가 클릭되면 호출되는 이벤트

    public event Action<PlanetTotalData> OnSelectButtonClick; // 선택하기 버튼 클릭 시 호출되는 이벤트 

    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }



    // 데이터 주입
    public void SetData(PlanetTotalData data)
    {
        planetData = data;

        if (cardImage != null && data != null)
        {
            cardImage.sprite = data.image;

            
        }
    }


    // 사용하기 버튼 클릭 시 족보 점수 업그레이드
    public void OnButtonClick()
    {
        if (planetData == null)
        {
            Debug.Log("행성 카드 데이터 없음");
            return;
        }
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ClickCard");

        // selectCount의 수와 비교하여 더 선택해야하는 지, 종료해야하는 지 판별


        // 0. 카드 상호작용 끄기
        OffRaycast();

        gameObject.GetComponent<RunPopUp>().OnExit();

        // 1. 위치 그대로 (임시)

        // 2. 업그레이드 적용
        PokerManager.Instance.ApplyPlanetUpgrade(planetData);

        // 3. 텍스트 띄우기
        OnSelectButtonClick.Invoke(planetData);

        // 4. 카드 비활성화
        gameObject.SetActive(false);

        // 5. 런 정보 텍스트 변경

        //Debug.Log("클릭중입니다 행성");
    }


    // 이벤트 트리거
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


    // |----



}
