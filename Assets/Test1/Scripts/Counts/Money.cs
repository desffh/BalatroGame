using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    // 전체 머니
    [SerializeField] private int totalMoney;
    
    public int TotalMoney => totalMoney;
    
    // 머니 텍스트 참조
    [SerializeField] MoneyText moneyText;

    // |------------------------------------

    private void Awake()
    {
        totalMoney = 10;
    }

    public void Start()
    {
        TextManager.Instance.UpdateText(7, totalMoney);
    }
    
    // 머니 텍스트 업데이트
    public void MoneyUpdate()
    {
        moneyText.UpdateText(totalMoney);
    }

    // |------------------------------------

    // 머니 리셋
    public void ReSetTotalMoney()
    {
        totalMoney = 10;
    }

    // 머니 획득
    public void AddMoney(int money)
    {
        totalMoney += money;

        // 획득한 머니 텍스트 업데이트
        moneyText.UpdatecashOutText(money);
    }

    // 머니 차감 
    public void MinusMoney(int money)
    {
        totalMoney -= money;

        if (totalMoney < 0)
        {
            totalMoney = 0;
        }
    }
}
