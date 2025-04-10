using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDelete : Singleton<HandDelete>
{

    // �ڵ� Ƚ�� & ������ Ƚ��

    [SerializeField] int hand;
    [SerializeField] int delete;

    public int Hand { get { return hand; } }
    public int Delete { get { return delete; } }
    


    protected override void Awake()
    {
        base.Awake();

        hand = 0;
        delete = 0;

    }

    private void Start()
    {
        // Ƚ�� �ʱ�ȭ
        Counting();
    }

    public void DeCountHand()
    {
        --hand;
        TextManager.Instance.UpdateText(3, hand);
    }
    public void DeCountDelete()
    {
        --delete;
        TextManager.Instance.UpdateText(4, delete);
    }

    // �ڵ� & ������ ����
    public void Counting()
    {
        hand = 4;
        delete = 4;

        TextManager.Instance.UpdateText(3, hand);
        TextManager.Instance.UpdateText(4, delete);
    }
}
