using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구매 불가 팝업 활성화 / 비활성화
public class ShopCardPanel : MonoBehaviour
{
    [SerializeField] GameObject Nobalance; // 잔액부족 팝업

    [SerializeField] GameObject OverJokerCount; // 조커 갯수 최대 팝업 (5개)

    [SerializeField] GameObject SellJokerPopup; // 조커 판매 시 팝업 

    [SerializeField] GameObject RerollPopup; // 리롤 안될 시 팝업
    private void Start()
    {
        Nobalance.SetActive(false);

        OverJokerCount.SetActive(false);

        SellJokerPopup.SetActive(false);
    
        RerollPopup.SetActive(false);
    }


    // |-------------------------

    // 구매 금액이 모자랄 때
    public void OnNoBalance()
    {
        Nobalance.SetActive(true);
    }

    public void DeleteNoBalance()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        Nobalance.SetActive(false);
    }

    // |-------------------------

    // 이미 보유중인 조커가 5개일 때
    public void OnOverJokerCount()
    {
        OverJokerCount.SetActive(true);
    }

    public void DeleteOverJokerCount()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OverJokerCount.SetActive(false);
    }

    // |-------------------------

    // 조커 판매하기 버튼을 눌렀을 때
    public void OnSellJokerPopup()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        SellJokerPopup.SetActive(true);
    }

    public void DeleteSellJokerPopup()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        SellJokerPopup.SetActive(false);
    }
    
    // |-------------------------

    // 구매 금액이 모자랄 때 (리롤)
    public void OnNoReroll()
    {
        RerollPopup.SetActive(true);
    }

    public void DeleteReroll()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        RerollPopup.SetActive(false);
    }

}
