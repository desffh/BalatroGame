using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class JokerCard : CardComponent
{
    [SerializeField] public JokerTotalData currentData;

    // UI 요소
    [SerializeField] private Image jokerImage;
    [SerializeField] private Text nameText;
    [SerializeField] private Text costText;
    [SerializeField] private Text abilityText;

    private JokerData data;
    private Sprite sprite;

    // 타입 인터페이스
    private IJokerEffect effect;

    public int unlockRound; // 등장 가능한 라운드


    private IPopupText jokerPopup;

    public bool isCalculating = false;

    // |------------------------------

    private void Awake()
    {
        jokerImage = GetComponent<Image>();
    }

    // 외부에서 조커 데이터 설정
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
        SetupEffect(); // 효과 객체 생성
    }

    // 조커 효과 세팅 (JokerEffectFactory 통해 생성)
    private void SetupEffect()
    {
        effect = JokerEffectFactory.Create(data);
    }

    // 조커 타입 반환
    public IJokerEffect GetEffect()
    {
        return effect;
    }


    // 효과 발동 - HoldManager에서 호출
    public bool ActivateEffect(JokerEffectContext context)
    {
        return effect?.ApplyEffect(context) ?? false; // 조커 category도 함께 전달
    }

    // 데이터 제공용
    public JokerData GetData() => data;
    public Sprite GetSprite() => sprite;

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

    private void SetupPopupComponent()
    {
        // 1. 기존 컴포넌트 제거
        var oldPopup = GetComponent<IPopupText>() as MonoBehaviour;
        if (oldPopup != null)
            Destroy(oldPopup);
    }





    public void DisableCard() => gameObject.SetActive(false);

    public override void OffCollider() => jokerImage.raycastTarget = false;
    public override void OnCollider() => jokerImage.raycastTarget = true;

    // UI에서 클릭될 때 호출
    public void OnMouse()
    {
        if (isCalculating) return; // 계산중이면 클릭 무시
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

    // 상점/내 조커 영역에서 클릭 시 처리
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
            Debug.Log("판매하기 버튼 활성화");
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
            Debug.Log("판매하기 종료");
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
