using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] int totalMoney;

    [SerializeField] MoneyText moneyText;

    

    public int TotalMoney
    {
        get { return totalMoney; }
    }

    private void Awake()
    {
        totalMoney = 0;
    }

    public void Start()
    {
        TextManager.Instance.UpdateText(7, totalMoney);
    }

    public void ReSetting()
    {
        totalMoney = 0;
        MoneyUpdate();
    }

    // 스테이지 클리어 시 머니 획득
    public void AddMoney(int money)
    {
        totalMoney += money;
        moneyText.UpdatecashText(money);
    }

    // 상점에서 구매 시 머니 차감 
    public void MinusMoney(int money)
    {
        if (totalMoney < 0)
        {
            totalMoney = 0;
        }
        totalMoney -= money;
    }

    public void MoneyUpdate()
    {
        moneyText.UpdateText(totalMoney);
    }
}
