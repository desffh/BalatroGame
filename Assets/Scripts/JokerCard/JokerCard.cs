using System;
using System.Collections;
using System.Collections.Generic;
using Google.GData.Spreadsheets;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

// IShopItem (��Ŀ �������̽�) �� ���� 

public class JokerCard : MonoBehaviour, IShopItem, IShopCard, IBuyCard, ISellCard, IInstantCard, ISelectCard
{
    [SerializeField] public JokerTotalData currentData;

    // UI ���
    [SerializeField] private Image jokerImage;

    [SerializeField] private GameObject myJokerPrefab; // �� ��Ŀ ī�� ������


    // |----


    // ��Ŀ���� ���� ������
    public JokerData data;

    private Sprite sprite;


    // |----


    // �ɷ� Ÿ�� �������̽�
    private IJokerEffect effect;

    // ��Ŀ Ÿ�� ��ȯ
    public IJokerEffect GetEffect() { return effect; }


    // ��Ŀ �˾� �������̽�
    private IPopupText jokerPopup;

    // ��Ŀ ��������� Ȯ�� -> ī�� Ŭ�� ���� ����
    public bool isCalculating = false;

    // ī�尡 ���ŵǾ����� Ȯ��
    private bool isPurchased = false;
    
    // ��Ŀ�� ���õǾ����� Ȯ��
    private bool isSelected = false;


    // |------------------------------


    // IShopCard ����

    public string Name => data.name;

    public int cost => data.cost;

    public Sprite Icon => sprite;


    // |------------------------------


    private ICardSelectionHandler selectionHandler;

    // �ڵ鷯 ����
    public void SetSelectionHandler(ICardSelectionHandler handler)
    {
        selectionHandler = handler;
    }


    // |------------------------------


    
    private void Awake()
    {
        jokerImage = GetComponent<Image>();
    }


    // ��Ŀ ������ ���� -> ������ ���� �� 
    public void SetJokerData(JokerTotalData joker)
    {
        currentData = new JokerTotalData(
            new JokerData(joker.baseData.name, joker.baseData.cost, joker.baseData.multiple, joker.baseData.require, joker.baseData.type),
            joker.image
        );

        data = currentData.baseData;
        sprite = currentData.image;

        jokerImage.sprite = sprite;

        PopupSetting();// ��Ŀ �˾� ����
        SetupEffect(); // ȿ�� ��ü ����
    }

    // |----

    // �̺�Ʈ Ʈ���� PointClick 
    public void OnMouse()
    {
        if (isCalculating) return; // ������̸� ��Ŀ Ŭ�� ����

        OnCardClicked();
    }

    // �̺�Ʈ Ʈ���� Enter
    public void OnEnterJoker()
    {
        AnimationManager.Instance.OnEnterJokerCard(gameObject);
    }

    // �̺�Ʈ Ʈ���� Exit
    public void OnExitJoker()
    {
        AnimationManager.Instance.OnExitJokerCard(gameObject);
    }

    // |----


    // IBuyCard ���� - �����ϱ� ��ư�� ������ �� ȣ�� (���� ��ġ, �� ī��, �ڵ鷯)
    public void OnBuy(Transform spawnParent, MyJokerCard cardList, ICardSelectionHandler handler)
    {
        // 1. �� ����
        StateManager.Instance.moneyViewSetting.Remove(cost);


        // 2. ��Ŀ ī�� ����
        GameObject obj = Instantiate(GetPrefab(), spawnParent);
        JokerCard newCard = obj.GetComponent<JokerCard>();
        newCard.SetJokerData(currentData); // ���� ������ ��Ŀ�� ������ ����


        // 3. �ڵ鷯 ����
        newCard.SetSelectionHandler(handler);


        // 4. �� ī�� ����Ʈ ���
        cardList.AddJokerCard(newCard);


        // 5. ������ ī�� ��Ȱ��ȭ
        DisableCard();


        // 6. ����
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");


        // ���ŵ� ó��
        newCard.MarkAsPurchased();
    }


    // |----


    // ISellCard ���� - �Ǹ��ϱ� ��ư�� ������ ��
    public void OnSell()
    {
        // 1. ȿ�� ���� ó�� -> ���� �������̽��� �ִٸ�
        if (effect is IExitEffect exit)
        {
            exit.ExitEffect(this);
        }


        // 2. ī�� ������ �ٽ� �߰�
        var manager = FindObjectOfType<JokerManager>();
        manager.PushData(currentData);


        // 3. �� ī�� ����Ʈ���� ����
        var myCards = FindObjectOfType<MyJokerCard>();
        myCards.RemoveJokerCard(this);


        // 4. ī�� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);


        // 5. �� ȯ��
        int refund = cost / 2;
        StateManager.Instance.moneyViewSetting.Add(refund);


        // 6. ����
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");
    }


    // |----
 

    // IInstantCard ����

    public void InstantiateCard(Transform parent)
    {
        var newCard = Instantiate(GetPrefab(), parent);
        var cardScript = newCard.GetComponent<JokerCard>();
        cardScript.SetJokerData(currentData);
    }

    // Shop���� ���� �� ���� �� ������
    public GameObject GetPrefab() { return myJokerPrefab; }

    // |----


    // ISelectCard ����

    public bool CanBeSold => isPurchased;  // ������ �� ������ �Ǹ� ����
    public bool IsInPlayerInventory => IsInMyJokerPanel();


    // |----


    // ȿ�� �ߵ� - HoldManager���� ȣ��
    public bool ActivateEffect(JokerEffectContext context)
    {
        return effect?.ApplyEffect(context) ?? false; // ��Ŀ category�� �Բ� ����
    }


    private void SetupPopupComponent()
    {
        // 1. ���� ������Ʈ ����
        var oldPopup = GetComponent<IPopupText>() as MonoBehaviour;
        if (oldPopup != null)
            Destroy(oldPopup);
    }


    public void DisableCard() => gameObject.SetActive(false);


    public void OnCardClicked()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        if (selectionHandler == null)
        {
            Debug.LogWarning("���� �ڵ鷯�� �������� ����");
            return;
        }
        if (isSelected) // �̹� ���õ� ī����
        {
            // �ڵ鷯�� ���õ� ��Ŀ(ISelectCardŸ��)�� ����

            selectionHandler.OnCardDeselected(this);

            isSelected = false;
        }
        else
        {
            selectionHandler.OnCardSelected(this);

            isSelected = true;
        }
    }


    // �� ��Ŀ ���� Ȯ��
    private bool IsInMyJokerPanel()
    {
        GameObject parent = GameObject.Find("JokersPanel");
        return parent != null && transform.IsChildOf(parent.transform);
    }


    // ���� ���� ����

    public void ForceUnselect()
    {
        isSelected = false;
        
    }


    // ī�尡 ���ŵǾ��ٸ� ȣ��
    public void MarkAsPurchased()
    {
        isPurchased = true;
    }


    // |---



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

    // ��Ŀ ȿ�� ���� (JokerEffectFactory ���� ����)
    private void SetupEffect()
    {
        effect = JokerEffectFactory.Create(data);
    }

}
