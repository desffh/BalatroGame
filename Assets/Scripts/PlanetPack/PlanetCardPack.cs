using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// �༺ ī������ ���� �� ����ó�� �� �ʿ�x

public class PlanetCardPack : MonoBehaviour, IBuyPlanetCard, IShopCard
{
    [SerializeField] PlanetPackUI planetPackUI;

    public int cost => planetPackUI.packCost;

    public RectTransform Transform => GetComponent<RectTransform>();


    public void OnBuy(Action<IShopCard> onCreated)
    {
        // 1. �� ����
        StateManager.Instance.moneyViewSetting.Remove(cost);


        // 2. ������ ī�� ��Ȱ��ȭ
        DisableCard();

        // 4. ����
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        Debug.Log("�༺ī�带 �����߽��ϴ�");
    }


    public void DisableCard() => gameObject.SetActive(false);


}
