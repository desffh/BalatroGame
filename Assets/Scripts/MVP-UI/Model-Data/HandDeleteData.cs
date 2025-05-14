using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/HandDeleteData")]

// ��ũ���ͺ� ������Ʈ �������� ���� -> ���� & ���� ����
public class HandDeleteData : ScriptableObject
{
    [SerializeField] private int hand = 4;
    [SerializeField] private int delete = 4;

    public int Hand => hand;
    public int Delete => delete;


    // ī��Ʈ ����
    public void ResetCounts()
    {
        hand = 4;
        delete = 4;
    }

    // �ڵ� ����
    public void DeCountHand()
    {
        if (hand > 0)
        {
            --hand;
        }
    }

    // ������ ����
    public void DeCountDelete()
    {
        if (delete > 0)
        {
            --delete;
        }
    }

    // ������ �߰�
    public void UpCountDelete()
    {
        ++delete;
    }

    // �ڵ� ����
    public void HandSetting(int value)
    {
        hand = value;
    }

    public void DeleteSetting(int value)
    {
        delete = value;
    }
}