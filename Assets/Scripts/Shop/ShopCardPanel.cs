using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �Ұ� �˾� Ȱ��ȭ / ��Ȱ��ȭ
public class ShopCardPanel : MonoBehaviour
{
    [SerializeField] GameObject Nobalance; // �ܾ׺��� �˾�

    [SerializeField] GameObject OverJokerCount; // ��Ŀ ���� �ִ� �˾� (5��)

    [SerializeField] GameObject SellJokerPopup; // ��Ŀ �Ǹ� �� �˾� 

    [SerializeField] GameObject RerollPopup; // ���� �ȵ� �� �˾�
    private void Start()
    {
        Nobalance.SetActive(false);

        OverJokerCount.SetActive(false);

        SellJokerPopup.SetActive(false);
    
        RerollPopup.SetActive(false);
    }


    // |-------------------------

    // ���� �ݾ��� ���ڶ� ��
    public void OnNoBalance()
    {
        Nobalance.SetActive(true);
    }

    public void DeleteNoBalance()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

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
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OverJokerCount.SetActive(false);
    }

    // |-------------------------

    // ��Ŀ �Ǹ��ϱ� ��ư�� ������ ��
    public void OnSellJokerPopup()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        SellJokerPopup.SetActive(true);
    }

    public void DeleteSellJokerPopup()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        SellJokerPopup.SetActive(false);
    }
    
    // |-------------------------

    // ���� �ݾ��� ���ڶ� �� (����)
    public void OnNoReroll()
    {
        RerollPopup.SetActive(true);
    }

    public void DeleteReroll()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        RerollPopup.SetActive(false);
    }

}
