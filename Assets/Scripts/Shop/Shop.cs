using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// 상점 관리 - 조커 추가, 타로 카드 추가, 행성 카드 추가, 바우처 추가

public class Shop : MonoBehaviour
{
    [SerializeField] MyJokerCard myJokerCards; // myCards리스트를 보유한 스크립트

    // |----

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // 내 조커 카드 프리팹

    // |----

    // 행성카드팩
    [SerializeField] private List<PlanetPackUI> planetPackSlots;
    [SerializeField] private List<PlanetCardPackSO> planetPackDatas;

    // 타로카드팩
    [SerializeField] private List<TaroPackUI> taroPackSlots;
    [SerializeField] private List<TaroCardPackSO> taroPackDatas;


    // 상점 팝업 
    [SerializeField] ShopCardPanel jokerPanel;

    // |----

    [SerializeField] private List<JokerCard> shopJokers; // 상점에 있는 조커 카드 오브젝트 2개

    // 내 조커 생성 위치
    [SerializeField] GameObject jokerPacksTransform;

    // |----

    [SerializeField] Button buyButton;
    [SerializeField] Button sellButton;


    // 선택 가능한 공통 인터페이스로 받기

    [SerializeField] private IShopCard currentTarget;
    public IShopCard CurrentTarget => currentTarget;

    // |----

    // 투명 패널
    [SerializeField] GameObject emptyPanel; // 상점
    [SerializeField] GameObject fullScreenBlocker;// 판매

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

    // 상점이 열릴 때 호출 - 이벤트 구독자
    public void OpenShop()
    {
        JokerSetting();

        PlanetSetting();

        TaroSetting();
    }

    // | ----

    // 조커 카드 정보 세팅
    public void JokerSetting()
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


            shopJokers[i].gameObject.SetActive(true);

            // 이벤트 등록 -> 조커 카드를 클릭했을 때 

            var card = shopJokers[i];
            card.OnClicked += OnCardSelected;
        }

        jokerManager.ShuffleBuffer();
    }

    // 행성 카드팩 정보 세팅 
    public void PlanetSetting()
    {
        for (int i = 0; i < planetPackSlots.Count; i++)
        {
            planetPackSlots[i].Init(planetPackDatas[i]);

            planetPackSlots[i].gameObject.SetActive(true);

            PlanetCardPack cardPack = planetPackSlots[i].GetComponent<PlanetCardPack>();

            // 버튼 이벤트 등록
            cardPack.OnClicked += OnCardSelected;

            // 카드 구매 시 호출 될 이벤트
            planetPackOpened.Register(cardPack);
        }    
    }

    // 타로 카드팩 정보 세팅
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


// 구매하기 버튼 활성화
public void ShowBuyButton(IShopCard target)
    {
        OffSellButton();

        currentTarget = target;

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.Transform.position + new Vector3(170, 50, 0);

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true);
    }

    // 판매하기 버튼 활성화
    public void ONSellButton(IShopCard target)
    {
        OffBuyButton();

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.Transform.position + new Vector3(165, 50, 0);

        sellButton.gameObject.SetActive(true);
        fullScreenBlocker.SetActive(true);
    }


    // |----


    // 구매하기 버튼
    public void Buy()
    {
        if (currentTarget is IBuyCard buyer)
        {
            // 조커 카드 제한 조건 (ICanBeSold = 조커 카드만 가짐)
            if (buyer is ICanBeSold && myJokerCards.myCards.Count >= 5)
            {
                Debug.Log("조커 카드는 최대 5장까지만 보유할 수 있습니다.");
                jokerPanel.OnOverJokerCount(); // 최대 개수 알림
                return;
            }

            // 돈 확인
            if (StateManager.Instance.moneyViewSetting.GetMoney() < currentTarget.cost)
            {
                Debug.Log($"금액 부족");

                jokerPanel.OnNoBalance();

                return;
            }
            
            // OnBuy -> 카드 구매 로직 실행!
            buyer.OnBuy(jokerPacksTransform.transform, myJokerCards, card =>
            {
                if (card is IShopCard cards)
                {
                    // 버튼 활성화 이벤트
                    cards.OnClicked += OnCardSelected;
                }
            });


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
                // 조커 데이터 반환

                jokerManager.PushData(shopJokers[i].currentData);
            }
        }

        // 행성 이벤트 해제
        foreach (var pack in planetPackSlots)
        {
            PlanetCardPack cardPack = pack.GetComponent<PlanetCardPack>();

            planetPackOpened.Unregister(cardPack);

        }
        // 타로 이벤트 해제
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
            // 이벤트 해제
            card.OnClicked -= OnCardDeselected;
        }

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

    // ICardSelectionHandler 구현 

    public void OnCardSelected(IShopCard card) // 카드를 클릭했을 때
    {
        // IShopCard 인터페이스로 카드 판별
       
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

    public void OnCardDeselected(IShopCard card) // 투명 패널을 클릭했을 때 (버튼 숨기기)
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
    public void SetTarget(IShopCard card)
    {
        currentTarget = card;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }
}
