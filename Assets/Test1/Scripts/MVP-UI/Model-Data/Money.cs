using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/Money")]

// 전체 돈 관리 (돈 추가 / 돈 감소)
public class Money : ScriptableObject
{
    // 전체 머니
    [SerializeField] private int totalMoney = 10;
    
    public int TotalMoney => totalMoney;

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
