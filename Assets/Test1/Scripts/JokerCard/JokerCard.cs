using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public class JokerCard : CardComponent
{
    // �� ��Ŀ�� �Ҵ�� ��ü ������ ����
    [SerializeField] private JokerTotalData currentData;

    // UI ���
    [SerializeField] private Image jokerImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text abilityText;

    // |--------------------------------

    private JokerData data;
    private Sprite sprite;

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

    }

    // |-------------------------------------

    public JokerData GetData() => data;
    public Sprite GetSprite() => sprite;

    public void SetJokerData(JokerTotalData joker)
    {
        currentData = new JokerTotalData(
    new JokerData(joker.baseData.name, joker.baseData.cost, joker.baseData.multiple, joker.baseData.require),
    joker.image);

        data = currentData.baseData;
        sprite = currentData.image;

        PopupSetting();
    }

    public void PopupSetting()
    {
        if (jokerPopup != null)
        {
        jokerPopup.Initialize(currentData.baseData.name,
        currentData.baseData.require,
        currentData.baseData.multiple);
        }
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

    // |-------------------------------

    //public JokerData data; // ��Ŀ ī�� ����

    public void OnClickCard()
    {
        Shop shop = FindAnyObjectByType<Shop>();

        shop.ShowBuyButton(this);
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
