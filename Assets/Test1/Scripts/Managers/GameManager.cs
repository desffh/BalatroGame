using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public System.Action OnStageChanged; // 스테이지 클리어 시

    [SerializeField] private bool hold;

    protected override void Awake()
    {
        base.Awake();
        OnStageChanged += KardManager.Instance.SetupNextStage; // 여기서 이벤트 등록
    }

    // |-스테이지 플레이 상태 (스테이지1)-----------------------------------------------|

    [SerializeField] bool playState;

    [SerializeField]
    public bool PlayState
    {
        get { return playState; }
    }

    [SerializeField] Money money;

    public void PlayOn()
    {
        Debug.Log("PlayOn 호출");

        //playState = true;
        
        if (OnStageChanged != null)
        {
            OnStageChanged.Invoke();
        }
    }

    public void PlayOff()
    {
        Debug.Log("PlayOff 호출");

        //playState = false;

        StartCoroutine(PopUpManager.Instance.OnClearPopup());

        // 수정!!!!!!!! 결과 팝업이 뜨고 계산한 뒤 머니 추가로 
        money.AddMoney(Round.Instance.Money);
        money.MoneyUpdate();
    }


}
