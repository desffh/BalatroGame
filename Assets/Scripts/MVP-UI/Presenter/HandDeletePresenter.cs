using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

// �𵨰� �並 �����ͼ� ����, ���� ����
//
// �Ϲ� C# ��ũ��Ʈ�� ������ ���ӿ�����Ʈ�� �Ҵ� �Ұ��� -> �ܺδ� ����
//
// StateManager���� ȣ���Ͽ� �Ҵ����ֱ� 
public class HandDeletePresenter : IHandDeleteSetting
{
    private HandDeleteData data;
    private HandDeleteView view;

    public HandDeletePresenter(HandDeleteData data, HandDeleteView view)
    {
        this.data = data;
        this.view = view;

        // �ʱ� �� UI �ݿ�
        view.UpdateHand(data.Hand);
        view.UpdateDelete(data.Delete);
    }


    // �������̽� ����

    public void Reset()
    {
        // ���� : �ڵ�4 ������4
        data.ResetCounts();

        view.UpdateHand(data.Hand);
        view.UpdateDelete(data.Delete);
    }

    public void MinusHand()
    {
        data.DeCountHand();               
        view.UpdateHand(data.Hand);       
    }

    public void MinusDelete()
    {
        data.DeCountDelete();
        view.UpdateDelete(data.Delete);
    }

    public void PlusDelete()
    {
        data.UpCountDelete();
        view.UpdateDelete(data.Delete);
    }

    public int GetHand() => data.Hand;
    public int GetDelete() => data.Delete;

}
