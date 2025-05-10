using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

// 모델과 뷰를 가져와서 중재, 로직 수행
//
// 일반 C# 스크립트기 때문에 게임오브젝트에 할당 불가능 -> 외부는 가능
//
// StateManager에서 호출하여 할당해주기 
public class HandDeletePresenter : IHandDeleteSetting
{
    private HandDeleteData data;
    private HandDeleteView view;

    public HandDeletePresenter(HandDeleteData data, HandDeleteView view)
    {
        this.data = data;
        this.view = view;

        // 초기 값 UI 반영
        view.UpdateHand(data.Hand);
        view.UpdateDelete(data.Delete);
    }


    // 인터페이스 정의

    public void Reset()
    {
        // 리셋 : 핸드4 버리기4
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
