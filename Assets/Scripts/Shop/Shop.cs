using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


// 상점 관리 - 조커 추가, 타로 카드 추가, 행성 카드 추가, 바우처 추가

public class Shop : MonoBehaviour, ICardSelectionHandler
{
    [SerializeField] MyJokerCard myJokerCards; // myCards리스트를 보유한 스크립트

    // |----

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // 내 조커 카드 프리팹

    // |----

    // 상점 팝업 
    [SerializeField] ShopCardPanel jokerPanel;

    // |----

    [SerializeField] private List<JokerCard> shopJokers; // 상점에 있는 조커 카드 오브젝트 2개

    // 내 조커 생성 위치
    [SerializeField] GameObject jokerPacksTransform;

    // |----

    [SerializeField] Button buyButton;
    [SerializeField] Button sellButton;

    [SerializeField] private JokerCard currentTarget;

    public JokerCard CurrentTarget => currentTarget;

    // |----

    // 투명 패널
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

            // Shop은 ICardSelectionHandler 구현체
            shopJokers[i].SetSelectionHandler(this); 

            shopJokers[i].gameObject.SetActive(true);
        }

        jokerManager.ShuffleBuffer();
    }

    // | ----

    // 구매하기 버튼 활성화
    public void ShowBuyButton(JokerCard target)
    {
        OffSellButton();

        currentTarget = target;

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0);

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true);
    }

    // 판매하기 버튼 활성화
    public void ONSellButton(JokerCard target)
    {
        OffBuyButton();

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(165, 50, 0);

        sellButton.gameObject.SetActive(true);
        fullScreenBlocker.SetActive(true);
    }


    // |----


    // 구매하기 버튼
    public void Buy()
    {
        if (currentTarget is IBuyCard buyer)
        {
            // 돈 확인
            if (StateManager.Instance.moneyViewSetting.GetMoney() < currentTarget.cost)
            {
                Debug.Log($"금액 부족");

                jokerPanel.OnNoBalance();

                return;
            }

            // 조커카드는 생성 위치, 카드 리스트, 핸들러가 필요함
            buyer.OnBuy(jokerPacksTransform.transform, myJokerCards, this);

            OffBuyButton();
            currentTarget = null;
        }
    }

    // 판매하기 버튼
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

    // 상점 닫기

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
    // [버튼 이벤트] 
    //
    // 판매 팝업 닫기 -> 팝업 비활성화
    public void DeleteSellJoker()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        jokerPanel.DeleteSellJokerPopup();
        OnBlockerClicked();
    }

    // 판매하기 버튼을 눌렀을 때 -> 팝업 활성화
    public void OnSellJokerPopups()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        jokerPanel.OnSellJokerPopup();
        fullScreenBlocker.SetActive(false);
    }

    // 리롤 버튼
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

    // ICardSelectionHandler 구현 

    public void OnCardSelected(ISelectCard card) // 카드를 클릭했을 때
    {
        // ISelectCard 인터페이스로 카드 판별

        if (card is JokerCard target)
        {
            // currentTarget의 설정
            SetTarget(target);

            // 이미 구매했고, 내 조커 영역에 있다면
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

    public void OnCardDeselected(ISelectCard card) // 투명 패널을 클릭했을 때 (버튼 숨기기)
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

    // 구매버튼 & 투명 패널 해제
    public void OffBuyButton()
    {
        buyButton.gameObject.SetActive(false);
        emptyPanel.SetActive(false);
    }
    public void OffSellButton()
    {
        sellButton.gameObject.SetActive(false);
        fullScreenBlocker.SetActive(false); // 클릭 차단기 끄기
    }


    // |----

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

    // |----

    // 클릭된 카드인지 판별 
    public void SetTarget(JokerCard card)
    {
        currentTarget = card;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
}
