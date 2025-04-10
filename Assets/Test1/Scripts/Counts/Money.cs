using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] int totalMoney;

    public int TotalMoney
    {
        get { return totalMoney; }
        
        // ���� Ŭ���� �� �Ӵ� �߰�
        set
        {
            totalMoney += value;
        }
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
    }

}
