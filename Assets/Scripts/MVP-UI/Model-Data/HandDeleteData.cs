using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Model/HandDeleteData")]

// 스크립터블 오브젝트 형식으로 저장 -> 정보 & 로직 저장
public class HandDeleteData : ScriptableObject
{
    [SerializeField] private int hand = 4;
    [SerializeField] private int delete = 4;

    public int Hand => hand;
    public int Delete => delete;


    // 카운트 리셋
    public void ResetCounts()
    {
        hand = 4;
        delete = 4;
    }

    // 핸드 감소
    public void DeCountHand()
    {
        if (hand > 0)
        {
            --hand;
        }
    }

    // 버리기 감소
    public void DeCountDelete()
    {
        if (delete > 0)
        {
            --delete;
        }
    }

    // 버리기 추가
    public void UpCountDelete()
    {
        ++delete;
    }

    // 핸드 셋팅
    public void HandSetting(int value)
    {
        hand = value;
    }

    public void DeleteSetting(int value)
    {
        delete = value;
    }
}