using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDelete : Singleton<HandDelete>
{
    // �ڵ� Ƚ�� & ������ Ƚ��

    [SerializeField] int hand;
    [SerializeField] int delete;

    public int Hand => hand;
    public int Delete => delete;
    
    // |------------------------------

    private void Start()
    {
        ResetCounts();
    }

    // ī��Ʈ ����
    public void ResetCounts()
    {
        hand = 4;
        delete = 4;

        TextManager.Instance.UpdateText(3, hand);
        TextManager.Instance.UpdateText(4, delete);
    }

    // �ڵ� ����
    public void DeCountHand()
    {
        if (hand > 0)
        {
            --hand;
            TextManager.Instance.UpdateText(3, hand);
        }
    }
    
    // ������ ����
    public void DeCountDelete()
    {
        if (delete > 0)
        {
            --delete;
            TextManager.Instance.UpdateText(4, delete);
        }
    }

    // �ڵ� & ������ ���� (���̵� �߰� �� ȣ��)
    public void SetCounts(int handCount, int deleteCount)
    {
        hand = handCount;
        delete = deleteCount;

        TextManager.Instance.UpdateText(3, hand);
        TextManager.Instance.UpdateText(4, delete);
    }
}
