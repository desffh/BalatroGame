using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokerCard : CardComponent
{
    [SerializeField] public JokerTotalData currentData;

    // UI ���
    [SerializeField] private Image jokerImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text abilityText;

    private JokerData data;
    private Sprite sprite;
    private IJokerEffect effect;

    public int unlockRound; // ���� ������ ����

    [SerializeField] private MonoBehaviour popupComponent;
    private IPopupText jokerPopup;

    [SerializeField] private bool checkClick = false;

    private void Awake()
    {
        jokerImage = GetComponent<Image>();
        jokerPopup = popupComponent as IPopupText;
    }

    // �ܺο��� ��Ŀ ������ ����
    public void SetJokerData(JokerTotalData joker)
    {
        currentData = new JokerTotalData(
            new JokerData(joker.baseData.name, joker.baseData.cost, joker.baseData.multiple, joker.baseData.require, joker.baseData.type),
            joker.image
        );

        data = currentData.baseData;
        sprite = currentData.image;

        jokerImage.sprite = sprite;

        PopupSetting();
        SetupEffect(); // ȿ�� ��ü ����
    }

    // ��Ŀ ȿ�� ���� (JokerEffectFactory ���� ����)
    private void SetupEffect()
    {
        effect = JokerEffectFactory.Create(data);
    }

    // ȿ�� �ߵ� - HoldManager���� ȣ��
    public void ActivateEffect(List<Card> selectedCards, string currentHandType, HoldManager holdManager)
    {
        effect?.ApplyEffect(selectedCards, currentHandType, holdManager, data.type); // ��Ŀ category�� �Բ� ����
    }

    // ������ ������
    public JokerData GetData() => data;
    public Sprite GetSprite() => sprite;

    // �˾� �ؽ�Ʈ ����
    public void PopupSetting()
    {
        jokerPopup?.Initialize(
            data.name,
            data.require,
            data.multiple,
            data.cost
        );
    }

    public void DisableCard() => gameObject.SetActive(false);

    public override void OffCollider() => jokerImage.raycastTarget = false;
    public override void OnCollider() => jokerImage.raycastTarget = true;

    // UI���� Ŭ���� �� ȣ��
    public void OnMouse()
    {
        OnCardClicked();
    }

    public override void OnCardClicked()
    {
        Shop shop = FindAnyObjectByType<Shop>();
        if (shop == null) return;

        if (shop.CurrentTarget == this)
        {
            OffClickCard();
            shop.ClearTarget();
        }
        else
        {
            if (shop.CurrentTarget != null)
                shop.CurrentTarget.OffClickCard();

            OnClickCard();
            shop.SetTarget(this);
        }
    }

    // ����/�� ��Ŀ �������� Ŭ�� �� ó��
    public void OnClickCard()
    {
        var shop = FindAnyObjectByType<Shop>();
        if (shop == null) return;

        if (IsInShop())
        {
            shop.ShowBuyButton(this);
        }
        else if (IsInMyJokerPanel())
        {
            shop.ONSellButton(this);
            Debug.Log("�Ǹ��ϱ� ��ư Ȱ��ȭ");
        }
    }

    public void OffClickCard()
    {
        var shop = FindAnyObjectByType<Shop>();
        if (shop == null) return;

        if (IsInShop())
        {
            shop.OffBuyButton();
        }
        else if (IsInMyJokerPanel())
        {
            shop.OffSellButton();
            Debug.Log("�Ǹ��ϱ� ����");
        }
    }

    // ���� ���� Ȯ��
    private bool IsInShop()
    {
        GameObject obj = GameObject.Find("ShopCanvas");
        return obj != null && transform.IsChildOf(obj.transform);
    }

    // �� ��Ŀ ���� Ȯ��
    private bool IsInMyJokerPanel()
    {
        GameObject parent = GameObject.Find("JokersPanel");
        return parent != null && transform.IsChildOf(parent.transform);
    }
}
