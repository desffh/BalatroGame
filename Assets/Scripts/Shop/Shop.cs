using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

// 상점 관리 - 조커 추가, 타로 카드 추가, 행성 카드 추가, 바우처 추가

public class Shop : MonoBehaviour, ICardSelectionHandler
{
    // 조커 카드 타입
    [SerializeField] List<JokerCard> jokerCards;

    [SerializeField] List<JokerCard> shopjokerCards;

    // 조커가 생성될 위치 
    [SerializeField] GameObject jokerTransform;

    // |---------------------------------------------------------
    
    [SerializeField] MyJokerCard myJokerCards; // myCards리스트를 보유한 스크립트



    // |---------------------------------------------------------

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // 내 조커 카드 프리팹

    // |---------------------------------------------------------

    [SerializeField] Money money;

    [SerializeField] ShopJokerPanel jokerPanel;

    // |---------------------------------------------------------

    [SerializeField] private List<JokerCard> shopJokers; // 상점에 있는 조커 카드 오브젝트 2개


    [SerializeField] GameObject jokerPacksTransform;

    [SerializeField] GameObject emptyPanel;

    // |---------------------------------------------------------


    public Button buyButton; // 인스펙터에 연결

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
            // 조커가 하나도 없으면 상점에 조커 카드 비활성화
            if (jokerManager.jokerBuffer.Count == 0)
            {
                shopJokers[i].gameObject.SetActive(false);
                continue;
            }

            // 1. 조커 정보 가져오기
            JokerTotalData selected = jokerManager.PopData();


            if (selected == null)
            {
                shopJokers[i].gameObject.SetActive(false);
                continue;
            }

            shopJokers[i].SetJokerData(selected);
           
            shopJokers[i].SetSelectionHandler(this); // Shop은 ICardSelectionHandler 구현체

            shopJokers[i].gameObject.SetActive(true);
        }

        jokerManager.ShuffleBuffer();
    }


    // | --------------------------------------------------


    // 클릭된 카드인지 판별 
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

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // 원하는 오프셋

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true); // 클릭 차단기 켜기
    }


    // |------------------------------------

    [SerializeField] Button sellButton;

    public void ONSellButton(JokerCard target)
    {
        OffBuyButton();

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(165, 50, 0); // 원하는 오프셋

        sellButton.gameObject.SetActive(true);
        fullScreenBlocker.SetActive(true); // 클릭 차단기 켜기
    }
    public void OffSellButton()
    {
        sellButton.gameObject.SetActive(false);
        fullScreenBlocker.SetActive(false); // 클릭 차단기 끄기
    }



    // 구매하기
    public void Buy()
    {
        if (currentTarget is IBuyCard buyer)
        {
            // 조커카드는 생성 위치, 카드 리스트, 핸들러가 필요함
            buyer.OnBuy(jokerPacksTransform.transform, myJokerCards, this); // 이 Shop은 ICardSelectionHandler를 구현 중

            OffBuyButton();
            currentTarget = null;
        }
    }


    // 판매하기 
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

    // 판매 버튼 비활성화
    public void OnBlockerClicked()
    {
        if (currentTarget != null)
        {
            // 선택 해제 처리
            OnCardDeselected(currentTarget);
        }

    }

    // 구매 버튼 비활성화
    public void OnEmptyClicked()
    {
        if (currentTarget != null)
        {
            // 선택 해제 처리
            OnCardDeselected(currentTarget);
        }

    }
    public void OffBuyButton()
    {
        buyButton.gameObject.SetActive(false);
        emptyPanel.SetActive(false); // 클릭 차단기 끄기
        // 위치는 오프셋 정하기
    }

    public void DeleteSellJoker()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        jokerPanel.DeleteSellJokerPopup();
        OnBlockerClicked();
    }

    // 리롤 버튼을 누를 시
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
            // currentTarget의 설정을 위한 것
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
