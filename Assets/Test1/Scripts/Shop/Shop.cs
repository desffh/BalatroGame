using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

// 상점 관리 - 조커 추가, 타로 카드 추가, 행성 카드 추가, 바우처 추가

public class Shop : MonoBehaviour
{
    // 조커 카드 타입 -> 라운드가 증가할수록 높은 조커가 나오도록
    [SerializeField] List<JokerCard> jokerCards;

    [SerializeField] List<JokerCard> shopjokerCards;

    // 조커가 생성될 위치 
    [SerializeField] GameObject jokerTransform;

    // |---------------------------------------------------------
    
    [SerializeField] MyJokerCard myJokerCards; // myCards리스트를 보유한 스크립트

    public int currentRound;

    // |---------------------------------------------------------

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // 내 조커 카드 프리팹

    // |---------------------------------------------------------

    [SerializeField] Money money;

    [SerializeField] ShopJokerPanel jokerPanel;

    // |---------------------------------------------------------

    [SerializeField] private List<JokerCard> shopJokers; // 상점에 있는 조커 카드 오브젝트 2개





    private void Awake()
    {
        currentRound = Round.Instance.Enty;


        // |------------------------
        buyButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        emptyPanel.gameObject.SetActive(false);
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
            shopJokers[i].gameObject.SetActive(true);
        }

        jokerManager.ShuffleBuffer();
    }


    // | --------------------------------------------------

    public Button buyButton; // 인스펙터에 연결

    [SerializeField] private JokerCard currentTarget;

    public JokerCard CurrentTarget => currentTarget;

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

    [SerializeField] GameObject jokerPacksTransform;

    [SerializeField] GameObject emptyPanel;

    public void ShowBuyButton(JokerCard target)
    {
        currentTarget = target;

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // 원하는 오프셋

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true); // 클릭 차단기 켜기
    }

    public void OffBuyButton()
    {
        buyButton.gameObject.SetActive(false);
        emptyPanel.SetActive(false); // 클릭 차단기 끄기
        // 위치는 오프셋 정하기
    }
    public void OnEmptyClicked()
    {
        OffBuyButton();       
        currentTarget = null;  
    }

    // |------------------------------------

    [SerializeField] Button sellButton;

    public void ONSellButton(JokerCard target)
    {
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
        if (currentTarget == null) return;

        // 조커가 5개 이상 & 현재 조커 금액 미만 이면 구매 불가능 
        if(myJokerCards.Cards.Count >= 5)
        {
            jokerPanel.OnOverJokerCount();
            return;
        }
        else if(money.TotalMoney < currentTarget.currentData.baseData.cost)
        {
            jokerPanel.OnNoBalance();
            return;
        }
        
        // 현재 머니에서 차감
        money.MinusMoney(currentTarget.currentData.baseData.cost);

        // 머니 UI업데이트
        money.MoneyUpdate();

        // 데이터 복사
        JokerData data = currentTarget.GetData();
        Sprite sprite = currentTarget.GetSprite();

        // 내 조커 영역에 생성
        GameObject newCard = Instantiate(myJokerPrefab, jokerTransform.transform);
        JokerCard cardScript = newCard.GetComponent<JokerCard>();
        
        // 생성된 조커의 JokerCard 내부에 데이터 저장
        cardScript.SetJokerData(new JokerTotalData(data, sprite));

        // 내 조커 카드에 담기(정보만)
        myJokerCards.AddJokerCard(cardScript);

        // 상점 카드 비활성화
        currentTarget.DisableCard();


        // 버튼 숨김
        buyButton.gameObject.SetActive(false);
        currentTarget = null;

        emptyPanel.gameObject.SetActive(false);

        // 상점 리스트에서도 제거
        //shopJokers.Remove(currentTarget);
    }

    // 판매하기 
    public void Sell()
    {
        if (currentTarget == null) return;
        Debug.Log("판매되나요?");


        var data = currentTarget.GetData();

        jokerManager.PushData(currentTarget.currentData);

        // 1. 리스트에서 제거
        myJokerCards.RemoveJokerCard(currentTarget);

        currentTarget.gameObject.SetActive(false);

        // 2. 카드 오브젝트 비활성화
        //currentTarget.DisableCard();

        // 3. 돈 환급 (예: 원가의 50% 회수)
        int refundAmount = currentTarget.currentData.baseData.cost / 2;
        money.AddMoney(refundAmount);
        money.MoneyUpdate();

        // 4. 상태 초기화
        currentTarget = null;
        sellButton.gameObject.SetActive(false);
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
        jokerPanel.OnSellJokerPopup();
        fullScreenBlocker.SetActive(false);
    }

    public void OnBlockerClicked()
    {
        OffSellButton();       // 판매 버튼 비활성화
        currentTarget = null;  // 선택 카드 초기화
    }

    public void DeleteSellJoker()
    {
        jokerPanel.DeleteSellJokerPopup();
        OnBlockerClicked();
    }
}
