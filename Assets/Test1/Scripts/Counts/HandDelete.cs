using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDelete : Singleton<HandDelete>
{
    // 핸드 횟수 & 버리기 횟수

    [SerializeField] int hand;
    [SerializeField] int delete;

    public int Hand => hand;
    public int Delete => delete;
    
    // |------------------------------

    private void Start()
    {
        ResetCounts();
    }

    // 카운트 리셋
    public void ResetCounts()
    {
        hand = 4;
        delete = 4;

        TextManager.Instance.UpdateText(3, hand);
        TextManager.Instance.UpdateText(4, delete);
    }

    // 핸드 감소
    public void DeCountHand()
    {
        if (hand > 0)
        {
            --hand;
            TextManager.Instance.UpdateText(3, hand);
        }
    }
    
    // 버리기 감소
    public void DeCountDelete()
    {
        if (delete > 0)
        {
            --delete;
            TextManager.Instance.UpdateText(4, delete);
        }
    }

    // 핸드 & 버리기 셋팅 (난이도 추가 시 호출)
    public void SetCounts(int handCount, int deleteCount)
    {
        hand = handCount;
        delete = deleteCount;

        TextManager.Instance.UpdateText(3, hand);
        TextManager.Instance.UpdateText(4, delete);
    }
}
