using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 행성 카드팩은 구매 후 구매처리 할 필요x

public class PlanetCardPack : MonoBehaviour, IBuyPlanetCard, IShopCard
{
    [SerializeField] PlanetPackUI planetPackUI;

    public int cost => planetPackUI.packCost;

    public RectTransform Transform => GetComponent<RectTransform>();


    public void OnBuy(Action<IShopCard> onCreated)
    {
        // 1. 돈 차감
        StateManager.Instance.moneyViewSetting.Remove(cost);


        // 2. 구매한 카드 비활성화
        DisableCard();

        // 4. 사운드
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        Debug.Log("행성카드를 구매했습니다");
    }


    public void DisableCard() => gameObject.SetActive(false);


}
