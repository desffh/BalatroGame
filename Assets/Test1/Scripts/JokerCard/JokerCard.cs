using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public class JokerCard : CardComponent
{
    // �� ��Ŀ�� �Ҵ�� ��ü ������ ����
    [SerializeField] public JokerTotalData currentData;

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
        // ��Ŀ ���� ����
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
        if (IsInShop())
        {
            FindAnyObjectByType<Shop>().ShowBuyButton(this);
        }
        else if (IsInMyJokerPanel())
        {
            FindAnyObjectByType<Shop>()?.ONSellButton(this);
            Debug.Log("�Ǹ��ϱ�");
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
            Debug.Log("�Ǹ��ϱ� ����");

        }
    }

    private bool IsInShop()
    {
        GameObject obj = GameObject.Find("ShopCanvas");
        if (obj == null)
        {
            Debug.LogError("'JokerPanel' ������Ʈ�� ã�� �� �����ϴ�!");
            return false;
        }

        return transform.IsChildOf(obj.transform);
    }

    private bool IsInMyJokerPanel()
    {
        GameObject parent = GameObject.Find("JokersPanel");
        if (parent == null)
        {
            Debug.LogError("Joker' ������Ʈ�� ã�� �� �����ϴ�.");
            return false;
        }

        return transform.IsChildOf(parent.transform);
    }



}
