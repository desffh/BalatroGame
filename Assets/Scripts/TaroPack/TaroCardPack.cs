using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaroCardPack : MonoBehaviour, IBuyCard, IShopCard
{
    [SerializeField] public TaroPackUI taroPackUI;

    // 선택되었는지 확인
    private bool isSelected = false;

    public int cost => taroPackUI.packCost;

    public RectTransform Transform => GetComponent<RectTransform>();


    public event Action<IShopCard> OnClicked;

    // 카드 열기 실행
    public event Action<TaroCardPack> OnPackOpened;


    // shop에서 구매하기 버튼을 눌렀을 때 호출됨
    public void OnBuy(Transform parent, MyJokerCard list, Action<IShopCard> onCreated)
    {
        // 1. 돈 차감
        StateManager.Instance.moneyViewSetting.Remove(cost);

        // 2. 구매한 카드팩 비활성화
        DisableCard();

        // 3. 사운드
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");


        // 4. 새로운 창 + 애니메이션 이벤트 실행
        OnPackOpened?.Invoke(this);
    }


    // 카드를 클릭했을 때
    public void OnCardClicked()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        isSelected = !isSelected;

        OnClicked?.Invoke(this); // 이벤트 실행 -> Shop 

    }


    public void DisableCard() => gameObject.SetActive(false);
}
