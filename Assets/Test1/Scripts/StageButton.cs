using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageButton : MonoBehaviour
{

    [SerializeField] Button button;

    [SerializeField] Button nextEnty;

    [SerializeField] Canvas stagecanvas;

    [SerializeField] Canvas Entycanvas;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClick1()
    {
        Stage1Click(0);
    }
    public void OnClick2()
    {
        Stage1Click(1);
    }
    public void OnClick3()
    {
        Stage1Click(2);
    }
    public void Stage1Click(int stage)
    {
        if(stage == 0)
        {
            Round.Instance.EntyUp();
        }

        Round.Instance.RoundUp();

        // 현재 스테이지 목표 점수 설정
        Round.Instance.Score(stage);
        
        // 스테이지 플레이 시작
        GameManager.Instance.PlayOn();
        
        // 스테이지 창 비활성화
        stagecanvas.gameObject.SetActive(false);

        // 초기화
        KardManager.Instance.SetupNextStage();
    }


    // -------------------- 캐시 아웃 버튼 -----------------------

    public void NextEntyOn()
    {
        Entycanvas.gameObject.SetActive(false);
        stagecanvas.gameObject.SetActive(true);

        if(Round.Instance.CurrentScores == Round.Instance.stages[2])
        {
            Round.Instance.isStage();
            Round.Instance.ScoreSetting();
        }

    }

    public void NextEntyOff()
    {
        nextEnty.interactable = false;
    }
}
