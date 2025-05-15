using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

// ���� ���� - ��Ŀ �߰�, Ÿ�� ī�� �߰�, �༺ ī�� �߰�, �ٿ�ó �߰�

public class Shop : MonoBehaviour, ICardSelectionHandler
{
    // ��Ŀ ī�� Ÿ��
    [SerializeField] List<JokerCard> jokerCards;

    [SerializeField] List<JokerCard> shopjokerCards;

    // ��Ŀ�� ������ ��ġ 
    [SerializeField] GameObject jokerTransform;

    // |---------------------------------------------------------
    
    [SerializeField] MyJokerCard myJokerCards; // myCards����Ʈ�� ������ ��ũ��Ʈ



    // |---------------------------------------------------------

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // �� ��Ŀ ī�� ������

    // |---------------------------------------------------------

    [SerializeField] Money money;

    [SerializeField] ShopJokerPanel jokerPanel;

    // |---------------------------------------------------------

    [SerializeField] private List<JokerCard> shopJokers; // ������ �ִ� ��Ŀ ī�� ������Ʈ 2��


    [SerializeField] GameObject jokerPacksTransform;

    [SerializeField] GameObject emptyPanel;

    // |---------------------------------------------------------


    public Button buyButton; // �ν����Ϳ� ����

    [SerializeField] private JokerCard currentTarget;

    public JokerCard CurrentTarget => currentTarget;


    private void Awake()
    {
        // |------------------------
        buyButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        emptyPanel.gameObject.SetActive(false);

        jokerManager.ShuffleBuffer();
    }

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
           
            shopJokers[i].SetSelectionHandler(this); // Shop�� ICardSelectionHandler ����ü

            shopJokers[i].gameObject.SetActive(true);
        }

        jokerManager.ShuffleBuffer();
    }


    // | --------------------------------------------------


    // Ŭ���� ī������ �Ǻ� 
    public void SetTarget(JokerCard card)
    {
        currentTarget = card;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }

    // | --------------------------------------------------

    public void ShowBuyButton(JokerCard target)
    {
        OffSellButton();

        currentTarget = target;

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // ���ϴ� ������

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true); // Ŭ�� ���ܱ� �ѱ�
    }


    // |------------------------------------

    [SerializeField] Button sellButton;

    public void ONSellButton(JokerCard target)
    {
        OffBuyButton();

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(165, 50, 0); // ���ϴ� ������

        sellButton.gameObject.SetActive(true);
        fullScreenBlocker.SetActive(true); // Ŭ�� ���ܱ� �ѱ�
    }
    public void OffSellButton()
    {
        sellButton.gameObject.SetActive(false);
        fullScreenBlocker.SetActive(false); // Ŭ�� ���ܱ� ����
    }



    // �����ϱ�
    public void Buy()
    {
        if (currentTarget is IBuyCard buyer)
        {
            // ��Ŀī��� ���� ��ġ, ī�� ����Ʈ, �ڵ鷯�� �ʿ���
            buyer.OnBuy(jokerPacksTransform.transform, myJokerCards, this); // �� Shop�� ICardSelectionHandler�� ���� ��

            OffBuyButton();
            currentTarget = null;
        }
    }


    // �Ǹ��ϱ� 
    public void Sell()
    {
        if (currentTarget is ISellCard seller)
        {
            seller.OnSell();
            OffSellButton();
            currentTarget = null;
        }
    }


    public void CloseShop()
    {
        for(int i = 0; i < shopJokers.Count; i++)
        {
            if(shopJokers[i].gameObject.activeSelf == true)
            {
                jokerManager.PushData(shopJokers[i].currentData);
            }
        }
    }

    private void OnEnable()
    {
        ShopPanel.OnShopOpened += OpenShop;
        StageButton.OnShopCloseRequest += CloseShop;
    }

    private void OnDisable()
    {
        ShopPanel.OnShopOpened -= OpenShop;
        StageButton.OnShopCloseRequest -= CloseShop;
    }

    [SerializeField] private GameObject fullScreenBlocker;

    public void OnSellJokerPopups()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        jokerPanel.OnSellJokerPopup();
        fullScreenBlocker.SetActive(false);
    }

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
    public void OffBuyButton()
    {
        buyButton.gameObject.SetActive(false);
        emptyPanel.SetActive(false); // Ŭ�� ���ܱ� ����
        // ��ġ�� ������ ���ϱ�
    }

    public void DeleteSellJoker()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        jokerPanel.DeleteSellJokerPopup();
        OnBlockerClicked();
    }

    // ���� ��ư�� ���� ��
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

    public void OnCardSelected(ISelectCard card)
    {
        if (card is JokerCard target)
        {
            // currentTarget�� ������ ���� ��
            SetTarget(target);

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


    public void OnCardDeselected(ISelectCard card)
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

}
