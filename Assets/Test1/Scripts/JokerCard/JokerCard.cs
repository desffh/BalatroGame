using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class JokerCard : CardComponent
{
    [SerializeField] public JokerDataSO dataSO;

    [SerializeField] private string jokerName;
    [SerializeField] private int cost;
    [SerializeField] private int multiple;
    [SerializeField] private string require;

    [SerializeField] Image jokerimage;

    public int unlockRound; // 이 카드가 등장 가능한 최소 라운드

    private void Awake()
    {
        jokerName = dataSO.name;
        cost = dataSO.cost;
        multiple = dataSO.multiple;
        require = dataSO.require;

        jokerimage = GetComponent<Image>();

    }

    private void Start()
    {

    }


    public override void OffCollider()
    {
        jokerimage.raycastTarget = false;
    }

    public override void OnCollider()
    {
        jokerimage.raycastTarget = true;
    }

    // 마우스 클릭이 발생했을 때
    public override void OnMouse()
    {
        OnClickCard();
    }

    public void ButtonOff()
    {

    }

    public void ButtonOn()
    {

    }

    // |-------------------------------

    //public JokerData data; // 조커 카드 정보

    public void OnClickCard()
    {
        Shop shop = FindAnyObjectByType<Shop>();

        shop.ShowBuyButton(this);
    }

}
