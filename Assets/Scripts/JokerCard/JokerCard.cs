using System;
using System.Collections;
using System.Collections.Generic;
using Google.GData.Spreadsheets;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

// IShopItem (마커 인터페이스) 로 묶기 

public class JokerCard : MonoBehaviour, IShopItem, IShopCard, IBuyCard, ISellCard, IInstantCard, ISelectCard
{
    [SerializeField] public JokerTotalData currentData;

    // UI 요소
    [SerializeField] private Image jokerImage;

    [SerializeField] private GameObject myJokerPrefab; // 내 조커 카드 프리팹


    // |----


    // 조커들이 가진 데이터
    public JokerData data;

    private Sprite sprite;


    // |----


    // 능력 타입 인터페이스
    private IJokerEffect effect;

    // 조커 타입 반환
    public IJokerEffect GetEffect() { return effect; }


    // 조커 팝업 인터페이스
    private IPopupText jokerPopup;

    // 조커 계산중인지 확인 -> 카드 클릭 막기 위함
    public bool isCalculating = false;

    // 카드가 구매되었는지 확인
    private bool isPurchased = false;
    
    // 조커가 선택되었는지 확인
    private bool isSelected = false;


    // |------------------------------


    // IShopCard 구현

    public string Name => data.name;

    public int cost => data.cost;

    public Sprite Icon => sprite;


    // |------------------------------


    private ICardSelectionHandler selectionHandler;

    // 핸들러 셋팅
    public void SetSelectionHandler(ICardSelectionHandler handler)
    {
        selectionHandler = handler;
    }


    // |------------------------------


    
    private void Awake()
    {
        jokerImage = GetComponent<Image>();
    }


    // 조커 데이터 설정 -> 상점이 열릴 때 
    public void SetJokerData(JokerTotalData joker)
    {
        currentData = new JokerTotalData(
            new JokerData(joker.baseData.name, joker.baseData.cost, joker.baseData.multiple, joker.baseData.require, joker.baseData.type),
            joker.image
        );

        data = currentData.baseData;
        sprite = currentData.image;

        jokerImage.sprite = sprite;

        PopupSetting();// 조커 팝업 설정
        SetupEffect(); // 효과 객체 생성
    }

    // 카드가 구매되었다면 호출
    public void MarkAsPurchased()
    {
        isPurchased = true;
    }






    // 효과 발동 - HoldManager에서 호출
    public bool ActivateEffect(JokerEffectContext context)
    {
        return effect?.ApplyEffect(context) ?? false; // 조커 category도 함께 전달
    }





    private void SetupPopupComponent()
    {
        // 1. 기존 컴포넌트 제거
        var oldPopup = GetComponent<IPopupText>() as MonoBehaviour;
        if (oldPopup != null)
            Destroy(oldPopup);
    }





    public void DisableCard() => gameObject.SetActive(false);


    // UI에서 클릭될 때 호출
    public void OnMouse()
    {
        if (isCalculating) return; // 계산중이면 클릭 무시

        OnCardClicked();
    }

    public void OnCardClicked()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");


        if (selectionHandler == null)
        {
            Debug.LogWarning("[JokerCard] 선택 핸들러가 설정되지 않음.");
            return;
        }
        if (isSelected)
        {
            selectionHandler.OnCardDeselected(this);
            //OffSelected();
            isSelected = false;
        }
        else
        {
            selectionHandler.OnCardSelected(this);
            //OnSelected();
            isSelected = true;
        }
    }

    

    // 상점 영역 확인
    private bool IsInShop()
    {
        GameObject obj = GameObject.Find("ShopCanvas");
        return obj != null && transform.IsChildOf(obj.transform);
    }

    // 내 조커 영역 확인
    private bool IsInMyJokerPanel()
    {
        GameObject parent = GameObject.Find("JokersPanel");
        return parent != null && transform.IsChildOf(parent.transform);
    }


    // |----

    // 이벤트 트리거 Enter
    public void OnEnterJoker()
    {
        AnimationManager.Instance.OnEnterJokerCard(gameObject);
    }

    // 이벤트 트리거 Exit
    public void OnExitJoker()
    {
        AnimationManager.Instance.OnExitJokerCard(gameObject);
    }

    // |----


    // 구매하기 버튼을 눌렀을 때 호출 (생성 위치, 내 카드, 핸들러)
    public void OnBuy(Transform spawnParent, MyJokerCard cardList, ICardSelectionHandler handler)
    {
        // 1. 돈 확인
        if (StateManager.Instance.moneyViewSetting.GetMoney() < cost)
        {
            Debug.Log($"금액 부족");
            return;
        }

        // 2. 돈 차감
        StateManager.Instance.moneyViewSetting.Remove(cost);


        // 3. 조커 카드 생성
        GameObject obj = Instantiate(GetPrefab(), spawnParent);
        JokerCard newCard = obj.GetComponent<JokerCard>();
        newCard.SetJokerData(currentData); // 동적 생성된 조커에 데이터 주입


        // 4. 핸들러 주입
        newCard.SetSelectionHandler(handler);


        // 5. 내 카드 리스트 등록
        cardList.AddJokerCard(newCard);


        // 6. 구매한 카드 비활성화
        DisableCard();


        // 7. 사운드
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");


        // 구매됨 처리
        newCard.MarkAsPurchased();
    }


    // |----


    // 판매하기 버튼을 눌렀을 때
    public void OnSell()
    {
        // 1. 효과 종료 처리 -> 종료 인터페이스가 있다면
        if (effect is IExitEffect exit)
        {
            exit.ExitEffect(this);
        }


        // 2. 카드 데이터 다시 추가
        var manager = FindObjectOfType<JokerManager>();
        manager.PushData(currentData);


        // 3. 내 카드 리스트에서 제거
        var myCards = FindObjectOfType<MyJokerCard>();
        myCards.RemoveJokerCard(this);


        // 4. 카드 오브젝트 비활성화
        gameObject.SetActive(false);


        // 5. 돈 환급
        int refund = cost / 2;
        StateManager.Instance.moneyViewSetting.Add(refund);
     
        
        // 6. 사운드
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");
    }


    // |----

    public GameObject GetPrefab() { return myJokerPrefab; }



    public void InstantiateCard(Transform parent)
    {
        var newCard = GameObject.Instantiate(GetPrefab(), parent);
        var cardScript = newCard.GetComponent<JokerCard>();
        cardScript.SetJokerData(currentData);
    }

    public void OnSelected()
    {
        // 버튼 활성화 요청은 Shop이 처리하므로 여기선 시각 처리만
        GetComponent<Image>().color = Color.yellow;
    }

    public void OffSelected()
    {
        GetComponent<Image>().color = Color.white;
    }



    // |----------------------------

    public bool CanBeSold => isPurchased;  // 구매한 적 있으면 판매 가능
    public bool IsInPlayerInventory => IsInMyJokerPanel();


    // 선택 해제 전용

    public void ForceUnselect()
    {
        isSelected = false;
        
    }


    // |---

    // 팝업 텍스트 설정
    public void PopupSetting()
    {
        SetupPopupComponent();

        // 팝업 생성
        jokerPopup = JokerPopupTextFactory.Create(data.type, gameObject);

        if (jokerPopup != null)
        {
            jokerPopup.Initialize(data.name, data.require, data.multiple, data.cost);
        }
    }

    // 조커 효과 세팅 (JokerEffectFactory 통해 생성)
    private void SetupEffect()
    {
        effect = JokerEffectFactory.Create(data);
    }
}
