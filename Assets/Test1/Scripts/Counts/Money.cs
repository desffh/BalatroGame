using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    // ��ü �Ӵ�
    [SerializeField] private int totalMoney;
    
    public int TotalMoney => totalMoney;
    
    // �Ӵ� �ؽ�Ʈ ����
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
    
    // �Ӵ� �ؽ�Ʈ ������Ʈ
    public void MoneyUpdate()
    {
        moneyText.UpdateText(totalMoney);
    }

    // |------------------------------------

    // �Ӵ� ����
    public void ReSetTotalMoney()
    {
        totalMoney = 10;
    }

    // �Ӵ� ȹ��
    public void AddMoney(int money)
    {
        totalMoney += money;

        // ȹ���� �Ӵ� �ؽ�Ʈ ������Ʈ
        moneyText.UpdatecashOutText(money);
    }

    // �Ӵ� ���� 
    public void MinusMoney(int money)
    {
        totalMoney -= money;

        if (totalMoney < 0)
        {
            totalMoney = 0;
        }
    }
}
