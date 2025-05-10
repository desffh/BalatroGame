using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
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

    // Ÿ�� �������̽�
    private IJokerEffect effect;

    public int unlockRound; // ���� ������ ����


    private IPopupText jokerPopup;

    public bool isCalculating = false;

    // |------------------------------

    private void Awake()
    {
        jokerImage = GetComponent<Image>();
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

    // ��Ŀ Ÿ�� ��ȯ
    public IJokerEffect GetEffect()
    {
        return effect;
    }


    // ȿ�� �ߵ� - HoldManager���� ȣ��
    public bool ActivateEffect(JokerEffectContext context)
    {
        return effect?.ApplyEffect(context) ?? false; // ��Ŀ category�� �Բ� ����
    }

    // ������ ������
    public JokerData GetData() => data;
    public Sprite GetSprite() => sprite;

    // �˾� �ؽ�Ʈ ����
    public void PopupSetting()
    {
        SetupPopupComponent();

        // �˾� ����
        jokerPopup = JokerPopupTextFactory.Create(data.type, gameObject);

        if (jokerPopup != null)
        {
            jokerPopup.Initialize(data.name, data.require, data.multiple, data.cost);
        }
    }

    private void SetupPopupComponent()
    {
        // 1. ���� ������Ʈ ����
        var oldPopup = GetComponent<IPopupText>() as MonoBehaviour;
        if (oldPopup != null)
            Destroy(oldPopup);
    }





    public void DisableCard() => gameObject.SetActive(false);

    public override void OffCollider() => jokerImage.raycastTarget = false;
    public override void OnCollider() => jokerImage.raycastTarget = true;

    // UI���� Ŭ���� �� ȣ��
    public void OnMouse()
    {
        if (isCalculating) return; // ������̸� Ŭ�� ����
        OnCardClicked();
    }

    public override void OnCardClicked()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

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

    public void OnEnterJoker()
    {
        AnimationManager.Instance.OnEnterJokerCard(gameObject);
    }

    public void OnExitJoker()
    {
        AnimationManager.Instance.OnExitJokerCard(gameObject);
    }

    // |----------------------------

}
