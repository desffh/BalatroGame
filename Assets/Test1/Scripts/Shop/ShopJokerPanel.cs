using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �Ұ� �˾� Ȱ��ȭ / ��Ȱ��ȭ
public class ShopJokerPanel : MonoBehaviour
{
    [SerializeField] GameObject Nobalance; // �ܾ׺��� �˾�

    [SerializeField] GameObject OverJokerCount; // ��Ŀ ���� �ִ� �˾� (5��)

    [SerializeField] GameObject SellJokerPopup; // ��Ŀ �Ǹ� �� �˾� 
    private void Start()
    {
        Nobalance.SetActive(false);

        OverJokerCount.SetActive(false);

        SellJokerPopup.SetActive(false);
    }

    // |-------------------------

    // ���� �ݾ��� ���ڶ� ��
    public void OnNoBalance()
    {
        Nobalance.SetActive(true);
    }

    public void DeleteNoBalance()
    {
        Nobalance.SetActive(false);
    }

    // |-------------------------

    // �̹� �������� ��Ŀ�� 5���� ��
    public void OnOverJokerCount()
    {
        OverJokerCount.SetActive(true);
    }

    public void DeleteOverJokerCount()
    {
        OverJokerCount.SetActive(false);
    }

    // |-------------------------

    // ��Ŀ �Ǹ��ϱ� ��ư�� ������ ��
    public void OnSellJokerPopup()
    {
        SellJokerPopup.SetActive(true);
    }

    public void DeleteSellJokerPopup()
    {
        SellJokerPopup.SetActive(false);
    }

    
}
