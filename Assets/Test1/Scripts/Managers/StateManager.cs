using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// 게임에 사용되는 핸드, 버리기 / 머니 / 칩, 배수 관리 담당
public class StateManager : Singleton<StateManager>
{
    [SerializeField] HandDelete handDelete;

    [SerializeField] Money money;

    [SerializeField] MultipleChip multipleChip;

    public HandDelete HandDelete { get { return handDelete; } }

    public Money Money { get { return money; } }

    public MultipleChip MultipleChip { get { return multipleChip; } }


    // 카운트 리셋
    public void ResetCounts()
    {
        handDelete.ResetCounts();
    }

    // 핸드 감소
    public void DeCountHand()
    {
        handDelete.DeCountHand();
    }

    // 버리기 감소
    public void DeCountDelete()
    {
        handDelete.DeCountDelete();
    }

    // 버리기 추가
    public void UpCountDelete()
    {
        handDelete.UpCountDelete();
    }

}
