using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// ���ӿ� ���Ǵ� �ڵ�, ������ / �Ӵ� / Ĩ, ��� ���� ���
public class StateManager : Singleton<StateManager>
{
    [SerializeField] HandDelete handDelete;

    [SerializeField] Money money;

    [SerializeField] MultipleChip multipleChip;

    public HandDelete HandDelete { get { return handDelete; } }

    public Money Money { get { return money; } }

    public MultipleChip MultipleChip { get { return multipleChip; } }


    // ī��Ʈ ����
    public void ResetCounts()
    {
        handDelete.ResetCounts();
    }

    // �ڵ� ����
    public void DeCountHand()
    {
        handDelete.DeCountHand();
    }

    // ������ ����
    public void DeCountDelete()
    {
        handDelete.DeCountDelete();
    }

    // ������ �߰�
    public void UpCountDelete()
    {
        handDelete.UpCountDelete();
    }

}
