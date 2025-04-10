using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDelete : Singleton<HandDelete>
{

    // 핸드 횟수 & 버리기 횟수

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
        // 횟수 초기화
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

    // 핸드 & 버리기 셋팅
    public void Counting()
    {
        hand = 4;
        delete = 4;

        TextManager.Instance.UpdateText(3, hand);
        TextManager.Instance.UpdateText(4, delete);
    }
}
