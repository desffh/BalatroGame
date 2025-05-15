using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


// ���� ���� - ��Ŀ �߰�, Ÿ�� ī�� �߰�, �༺ ī�� �߰�, �ٿ�ó �߰�

public class Shop : MonoBehaviour, ICardSelectionHandler
{
    [SerializeField] MyJokerCard myJokerCards; // myCards����Ʈ�� ������ ��ũ��Ʈ

    // |----

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // �� ��Ŀ ī�� ������

    // |----

    // ���� �˾� 
    [SerializeField] ShopCardPanel jokerPanel;

    // |----

    [SerializeField] private List<JokerCard> shopJokers; // ������ �ִ� ��Ŀ ī�� ������Ʈ 2��

    // �� ��Ŀ ���� ��ġ
    [SerializeField] GameObject jokerPacksTransform;

    // |----

    [SerializeField] Button buyButton;
    [SerializeField] Button sellButton;

    [SerializeField] private JokerCard currentTarget;

    public JokerCard CurrentTarget => currentTarget;

    // |----

    // ���� �г�
    [SerializeField] GameObject emptyPanel;
    [SerializeField] GameObject fullScreenBlocker;


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

    public void OpenShop()
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

            // Shop�� ICardSelectionHandler ����ü
            shopJokers[i].SetSelectionHandler(this); 

            shopJokers[i].gameObject.SetActive(true);
        }

        jokerManager.ShuffleBuffer();
    }

    // | ----

    // �����ϱ� ��ư Ȱ��ȭ
    public void ShowBuyButton(JokerCard target)
    {
        OffSellButton();

        currentTarget = target;

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0);

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true);
    }

    // �Ǹ��ϱ� ��ư Ȱ��ȭ
    public void ONSellButton(JokerCard target)
    {
        OffBuyButton();

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(165, 50, 0);

        sellButton.gameObject.SetActive(true);
        fullScreenBlocker.SetActive(true);
    }


    // |----


    // �����ϱ� ��ư
    public void Buy()
    {
        if (currentTarget is IBuyCard buyer)
        {
            // �� Ȯ��
            if (StateManager.Instance.moneyViewSetting.GetMoney() < currentTarget.cost)
            {
                Debug.Log($"�ݾ� ����");

                jokerPanel.OnNoBalance();

                return;
            }

            // ��Ŀī��� ���� ��ġ, ī�� ����Ʈ, �ڵ鷯�� �ʿ���
            buyer.OnBuy(jokerPacksTransform.transform, myJokerCards, this);

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
                jokerManager.PushData(shopJokers[i].currentData);
            }
        }
    }

    private void OnDisable()
    {
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
        if (StateManager.Instance.moneyViewSetting.GetMoney() < 5)
        {
            jokerPanel.OnNoReroll();
            return;
        }

        StateManager.Instance.moneyViewSetting.Remove(5);

        CloseShop();
        OpenShop();
    }

    // |----

    // ICardSelectionHandler ���� 

    public void OnCardSelected(ISelectCard card) // ī�带 Ŭ������ ��
    {
        // ISelectCard �������̽��� ī�� �Ǻ�

        if (card is JokerCard target)
        {
            // currentTarget�� ����
            SetTarget(target);

            // �̹� �����߰�, �� ��Ŀ ������ �ִٸ�
            if (card.IsInPlayerInventory && card.CanBeSold)
            {
                ONSellButton(target);
            }
            else
            {
                ShowBuyButton(target);
            }
        }
    }

    public void OnCardDeselected(ISelectCard card) // ���� �г��� Ŭ������ �� (��ư �����)
    {
        if (card is JokerCard target && target == currentTarget)
        {
            if (card.IsInPlayerInventory && card.CanBeSold)
            {
                OffSellButton();
            }
            else
            {
                OffBuyButton();
            }

            target.ForceUnselect();
            ClearTarget();
        }
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
    public void SetTarget(JokerCard card)
    {
        currentTarget = card;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
}
