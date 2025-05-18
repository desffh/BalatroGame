using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/Money")]

// ��ü �� ���� (�� �߰� / �� ����)
public class Money : ScriptableObject
{
    // ��ü �Ӵ�
    [SerializeField] private int totalMoney = 5;
    
    public int TotalMoney => totalMoney;

    // |------------------------------------

    // �Ӵ� ����
    public void ReSetTotalMoney()
    {
        totalMoney = 5;
    }

    // �Ӵ� ȹ��
    public void AddMoney(int money)
    {
        totalMoney += money;
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
