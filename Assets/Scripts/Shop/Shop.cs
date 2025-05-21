using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// ���� ���� - ��Ŀ �߰�, Ÿ�� ī�� �߰�, �༺ ī�� �߰�, �ٿ�ó �߰�

public class Shop : MonoBehaviour
{
    [SerializeField] MyJokerCard myJokerCards; // myCards����Ʈ�� ������ ��ũ��Ʈ

    // |----

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // �� ��Ŀ ī�� ������

    // |----

    // �༺ī����
    [SerializeField] private List<PlanetPackUI> planetPackSlots;
    [SerializeField] private List<PlanetCardPackSO> planetPackDatas;

    // Ÿ��ī����
    [SerializeField] private List<TaroPackUI> taroPackSlots;
    [SerializeField] private List<TaroCardPackSO> taroPackDatas;


    // ���� �˾� 
    [SerializeField] ShopCardPanel jokerPanel;

    // |----

    [SerializeField] private List<JokerCard> shopJokers; // ������ �ִ� ��Ŀ ī�� ������Ʈ 2��

    // �� ��Ŀ ���� ��ġ
    [SerializeField] GameObject jokerPacksTransform;

    // |----

    [SerializeField] Button buyButton;
    [SerializeField] Button sellButton;


    // ���� ������ ���� �������̽��� �ޱ�

    [SerializeField] private IShopCard currentTarget;
    public IShopCard CurrentTarget => currentTarget;

    // |----

    // ���� �г�
    [SerializeField] GameObject emptyPanel; // ����
    [SerializeField] GameObject fullScreenBlocker;// �Ǹ�

    [SerializeField] PlanetPackOpened planetPackOpened;
    [SerializeField] TaroPackOpened TaroPackOpened;

    private void Awake()
    {
        // |------------------------
        buyButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ShopPanel.OnShopOpened += OpenShop;
        StageButton.OnShopCloseRequest += CloseShop;

        
    }

    private void Start()
    {
        emptyPanel.gameObject.SetActive(false);

        jokerManager.ShuffleBuffer();
    }

    // |----

    // ������ ���� �� ȣ�� - �̺�Ʈ ������
    public void OpenShop()
    {
        JokerSetting();

        PlanetSetting();

        TaroSetting();
    }

    // | ----

    // ��Ŀ ī�� ���� ����
    public void JokerSetting()
    {
        for (int i = 0; i < shopJokers.Count; i++)
        {
            // ��Ŀ�� �ϳ��� ������ ������ ��Ŀ ī�� ��Ȱ��ȭ
            if (jokerManager.jokerBuffer.Count == 0)
            {
                shopJokers[i].gameObject.SetActive(false);
                continue;
            }

            // 1. ��Ŀ ���� ��������
            JokerTotalData selected = jokerManager.PopData();

            if (selected == null)
            {
                shopJokers[i].gameObject.SetActive(false);
                continue;
            }

            shopJokers[i].SetJokerData(selected);


            shopJokers[i].gameObject.SetActive(true);

            // �̺�Ʈ ��� -> ��Ŀ ī�带 Ŭ������ �� 

            var card = shopJokers[i];
            card.OnClicked += OnCardSelected;
        }

        jokerManager.ShuffleBuffer();
    }

    // �༺ ī���� ���� ���� 
    public void PlanetSetting()
    {
        for (int i = 0; i < planetPackSlots.Count; i++)
        {
            planetPackSlots[i].Init(planetPackDatas[i]);

            planetPackSlots[i].gameObject.SetActive(true);

            PlanetCardPack cardPack = planetPackSlots[i].GetComponent<PlanetCardPack>();

            // ��ư �̺�Ʈ ���
            cardPack.OnClicked += OnCardSelected;

            // ī�� ���� �� ȣ�� �� �̺�Ʈ
            planetPackOpened.Register(cardPack);
        }    
    }

    // Ÿ�� ī���� ���� ����
    public void TaroSetting()
    {
        for (int i = 0; i < taroPackSlots.Count; i++)
        {
            taroPackSlots[i].Init(taroPackDatas[i]);

            taroPackSlots[i].gameObject.SetActive(true);

            TaroCardPack cardpack = taroPackSlots[i].GetComponent<TaroCardPack>();

            cardpack.OnClicked += OnCardSelected;

            TaroPackOpened.Register(cardpack);
        }
    }
    


// |----


// �����ϱ� ��ư Ȱ��ȭ
public void ShowBuyButton(IShopCard target)
    {
        OffSellButton();

        currentTarget = target;

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.Transform.position + new Vector3(170, 50, 0);

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true);
    }

    // �Ǹ��ϱ� ��ư Ȱ��ȭ
    public void ONSellButton(IShopCard target)
    {
        OffBuyButton();

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.Transform.position + new Vector3(165, 50, 0);

        sellButton.gameObject.SetActive(true);
        fullScreenBlocker.SetActive(true);
    }


    // |----


    // �����ϱ� ��ư
    public void Buy()
    {
        if (currentTarget is IBuyCard buyer)
        {
            // ��Ŀ ī�� ���� ���� (ICanBeSold = ��Ŀ ī�常 ����)
            if (buyer is ICanBeSold && myJokerCards.myCards.Count >= 5)
            {
                Debug.Log("��Ŀ ī��� �ִ� 5������� ������ �� �ֽ��ϴ�.");
                jokerPanel.OnOverJokerCount(); // �ִ� ���� �˸�
                return;
            }

            // �� Ȯ��
            if (StateManager.Instance.moneyViewSetting.GetMoney() < currentTarget.cost)
            {
                Debug.Log($"�ݾ� ����");

                jokerPanel.OnNoBalance();

                return;
            }
            
            // OnBuy -> ī�� ���� ���� ����!
            buyer.OnBuy(jokerPacksTransform.transform, myJokerCards, card =>
            {
                if (card is IShopCard cards)
                {
                    // ��ư Ȱ��ȭ �̺�Ʈ
                    cards.OnClicked += OnCardSelected;
                }
            });


            OffBuyButton();
            currentTarget = null;
        }
    }

    // �Ǹ��ϱ� ��ư
    public void Sell()
    {
        if (currentTarget is ISellCard seller)
        {
            seller.OnSell();
            OffSellButton();
            currentTarget = null;
        }
    }

    // |----

    // ���� �ݱ�

    public void CloseShop()
    {
        for (int i = 0; i < shopJokers.Count; i++)
        {
            if (shopJokers[i].gameObject.activeSelf == true)
            {
                // ��Ŀ ������ ��ȯ

                jokerManager.PushData(shopJokers[i].currentData);
            }
        }

        // �༺ �̺�Ʈ ����
        foreach (var pack in planetPackSlots)
        {
            PlanetCardPack cardPack = pack.GetComponent<PlanetCardPack>();

            planetPackOpened.Unregister(cardPack);

        }
        // Ÿ�� �̺�Ʈ ����
        foreach (var pack in taroPackSlots)
        {
            TaroCardPack cardPack = pack.GetComponent<TaroCardPack>();

            TaroPackOpened.Unregister(cardPack);

        }

    }

    private void OnDisable()
    {
        foreach (var card in shopJokers)
        {
            // �̺�Ʈ ����
            card.OnClicked -= OnCardDeselected;
        }

        ShopPanel.OnShopOpened -= OpenShop;

        StageButton.OnShopCloseRequest -= CloseShop;


    }

    // |----
    //
    // [��ư �̺�Ʈ] 
    //
    // �Ǹ� �˾� �ݱ� -> �˾� ��Ȱ��ȭ
    public void DeleteSellJoker()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        jokerPanel.DeleteSellJokerPopup();
        OnBlockerClicked();
    }

    // �Ǹ��ϱ� ��ư�� ������ �� -> �˾� Ȱ��ȭ
    public void OnSellJokerPopups()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        jokerPanel.OnSellJokerPopup();
        fullScreenBlocker.SetActive(false);
    }

    // ���� ��ư
    public void Reroll()
    {
        if (StateManager.Instance.moneyViewSetting.GetMoney() < 2)
        {
            jokerPanel.OnNoReroll();
            return;
        }

        StateManager.Instance.moneyViewSetting.Remove(2);

        CloseShop();
        OpenShop();
    }

    // |----

    // ICardSelectionHandler ���� 

    public void OnCardSelected(IShopCard card) // ī�带 Ŭ������ ��
    {
        // IShopCard �������̽��� ī�� �Ǻ�
       
        SetTarget(card);

        if (card is ICanBeSold sellable && sellable.IsInPlayerInventory && sellable.CanBeSold)
        {
            ONSellButton(card);
        }
        else
        {
            ShowBuyButton(card);
        }
    }

    public void OnCardDeselected(IShopCard card) // ���� �г��� Ŭ������ �� (��ư �����)
    {

        if (card is ICanBeSold sellable && sellable.IsInPlayerInventory && sellable.CanBeSold)
        {
            OffSellButton();
        }
        else
        {
            OffBuyButton();
        }

        if (card is JokerCard target)
        {
            target.ForceUnselect();
        }

        ClearTarget();

    }

    // |----

    // ���Ź�ư & ���� �г� ����
    public void OffBuyButton()
    {
        buyButton.gameObject.SetActive(false);
        emptyPanel.SetActive(false);
    }
    public void OffSellButton()
    {
        sellButton.gameObject.SetActive(false);
        fullScreenBlocker.SetActive(false); // Ŭ�� ���ܱ� ����
    }


    // |----

    // �Ǹ� ��ư ��Ȱ��ȭ
    public void OnBlockerClicked()
    {
        if (currentTarget != null)
        {
            // ���� ���� ó��
            OnCardDeselected(currentTarget);
        }

    }

    // ���� ��ư ��Ȱ��ȭ
    public void OnEmptyClicked()
    {
        if (currentTarget != null)
        {
            // ���� ���� ó��
            OnCardDeselected(currentTarget);
        }
    }

    // |----

    // Ŭ���� ī������ �Ǻ� 
    public void SetTarget(IShopCard card)
    {
        currentTarget = card;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
}
