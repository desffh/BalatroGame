using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JokerCard : CardComponent
{
    // �� ��Ŀ�� �Ҵ�� ��ü ������ ����
    private JokerTotalData currentData;

    // UI ���
    [SerializeField] private Image jokerImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text abilityText;
    [SerializeField] private Button buyButton;

    [SerializeField] Shop shop;

    // ���� �� ���
    private string jokername;
    private int jokercost;
    private int jokermultiple;
    private string jokerrequire;
    
    // |--------------------------------

    public int unlockRound; // �� ī�尡 ���� ������ �ּ� ����

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
            Debug.LogWarning("jokerPopup�� �Ҵ���� �ʾҽ��ϴ�!");
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

    // ���콺 Ŭ���� �߻����� ��
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

    //public JokerData data; // ��Ŀ ī�� ����

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
