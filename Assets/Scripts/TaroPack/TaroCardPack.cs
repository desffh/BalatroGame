using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaroCardPack : MonoBehaviour, IBuyCard, IShopCard
{
    [SerializeField] public TaroPackUI taroPackUI;

    // ���õǾ����� Ȯ��
    private bool isSelected = false;

    public int cost => taroPackUI.packCost;

    public RectTransform Transform => GetComponent<RectTransform>();


    public event Action<IShopCard> OnClicked;

    // ī�� ���� ����
    public event Action<TaroCardPack> OnPackOpened;


    // shop���� �����ϱ� ��ư�� ������ �� ȣ���
    public void OnBuy(Transform parent, MyJokerCard list, Action<IShopCard> onCreated)
    {
        // 1. �� ����
        StateManager.Instance.moneyViewSetting.Remove(cost);

        // 2. ������ ī���� ��Ȱ��ȭ
        DisableCard();

        // 3. ����
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");


        // 4. ���ο� â + �ִϸ��̼� �̺�Ʈ ����
        OnPackOpened?.Invoke(this);
    }


    // ī�带 Ŭ������ ��
    public void OnCardClicked()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        isSelected = !isSelected;

        OnClicked?.Invoke(this); // �̺�Ʈ ���� -> Shop 

    }


    public void DisableCard() => gameObject.SetActive(false);
}
