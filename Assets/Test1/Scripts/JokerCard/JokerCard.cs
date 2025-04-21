using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JokerCard : CardComponent
{
    // 이 조커에 할당된 전체 데이터 정보
    private JokerTotalData currentData;

    // UI 요소
    [SerializeField] private Image jokerImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text abilityText;
    [SerializeField] private Button buyButton;

    [SerializeField] Shop shop;

    // 저장 된 요소
    private string jokername;
    private int jokercost;
    private int jokermultiple;
    private string jokerrequire;
    
    // |--------------------------------

    public int unlockRound; // 이 카드가 등장 가능한 최소 라운드

    // |--------------------------------
    [SerializeField] private MonoBehaviour popupComponent;

    private IPopupText jokerPopup;


    private void Awake()
    {
        jokerImage = GetComponent<Image>();

        jokerPopup = popupComponent as IPopupText;

    }

    private void Start()
    {
        if (jokerPopup != null)
        {
            jokerPopup.Initialize(currentData.baseData.name, 
                currentData.baseData.require, currentData.baseData.multiple);
        }
        else
        {
            Debug.LogWarning("jokerPopup이 할당되지 않았습니다!");
        }
    }

    // |-------------------------------------

    public void SetJokerData(JokerTotalData data)
    {
        currentData = data;

        if (data.image != null)
            jokerImage.sprite = data.image;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => shop.ShowBuyButton(this));
    }

    public JokerTotalData GetCurrentData() => currentData;

    public void DisableCard() => gameObject.SetActive(false);

    // |-------------------------------------

    public override void OffCollider()
    {
        jokerImage.raycastTarget = false;
    }

    public override void OnCollider()
    {
        jokerImage.raycastTarget = true;
    }

    [SerializeField] bool checkClick = false;

    // 마우스 클릭이 발생했을 때
    public override void OnMouse()
    {
        if (checkClick == false)
        {
            OnClickCard();
            checkClick = true;
        }

        else if (checkClick == true)
        {
            OffClickCard();
            checkClick = false;
        }
    }

    public void ButtonOff()
    {

    }

    public void ButtonOn()
    {

    }

    // |-------------------------------

    //public JokerData data; // 조커 카드 정보

    public void OnClickCard()
    {
        Shop shop = FindAnyObjectByType<Shop>();

        shop.ShowBuyButton(this);

        gameObject.SetActive(false);
    }

    public void OffClickCard()
    { 
        Shop shop = FindAnyObjectByType<Shop>();

        if(shop != null)
        {
            shop.OffBuyButton();
        }
    }

}
