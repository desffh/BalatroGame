using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public class JokerCard : CardComponent
{
    // 이 조커에 할당된 전체 데이터 정보
    [SerializeField] public JokerTotalData currentData;

    // UI 요소
    [SerializeField] private Image jokerImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text abilityText;

    // |--------------------------------

    private JokerData data;
    private Sprite sprite;

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

    }

    // |-------------------------------------

    public JokerData GetData() => data;
    public Sprite GetSprite() => sprite;

    public void SetJokerData(JokerTotalData joker)
    {
        // 조커 정보 셋팅
        currentData = new JokerTotalData(
    new JokerData(joker.baseData.name, joker.baseData.cost, joker.baseData.multiple, joker.baseData.require),
    joker.image);

        data = currentData.baseData;
        sprite = currentData.image;

        jokerImage.sprite = sprite;

        PopupSetting();
    }

    public void PopupSetting()
    {
        if (jokerPopup != null)
        {
        jokerPopup.Initialize(currentData.baseData.name,
        currentData.baseData.require,
        currentData.baseData.multiple,
        currentData.baseData.cost);
        }
    }




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

    // |-------------------------------

    //public JokerData data; // 조커 카드 정보

    public void OnClickCard()
    {
        if (IsInShop())
        {
            FindAnyObjectByType<Shop>().ShowBuyButton(this);
        }
        else if (IsInMyJokerPanel())
        {
            FindAnyObjectByType<Shop>()?.ONSellButton(this);
            Debug.Log("판매하기");
        }
    }

    public void OffClickCard()
    {
        if (IsInShop())
        {
            FindAnyObjectByType<Shop>().OffBuyButton();
        }
        else if (IsInMyJokerPanel())
        {
            FindAnyObjectByType<Shop>().OffSellButton();
            Debug.Log("판매하기 종료");

        }
    }

    private bool IsInShop()
    {
        GameObject obj = GameObject.Find("ShopCanvas");
        if (obj == null)
        {
            Debug.LogError("'JokerPanel' 오브젝트를 찾을 수 없습니다!");
            return false;
        }

        return transform.IsChildOf(obj.transform);
    }

    private bool IsInMyJokerPanel()
    {
        GameObject parent = GameObject.Find("JokersPanel");
        if (parent == null)
        {
            Debug.LogError("Joker' 오브젝트를 찾을 수 없습니다.");
            return false;
        }

        return transform.IsChildOf(parent.transform);
    }



}
