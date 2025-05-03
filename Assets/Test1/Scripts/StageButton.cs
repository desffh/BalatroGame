using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StageButton : MonoBehaviour
{

    [SerializeField] Button button;

    [SerializeField] Button nextEnty;

    [SerializeField] Canvas stagecanvas; // 엔티3개 나오는 캔버스

    [SerializeField] GameObject Entycanvas; // 결과창 캔버스

    [SerializeField] GameObject ShopPanel; // 상점 캔버스



    public static event System.Action OnShopCloseRequest;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnClick1()
    {
        SoundManager.Instance.ButtonClick();

        Stage1Click(0);
    }
    public void OnClick2()
    {
        SoundManager.Instance.ButtonClick();

        Stage1Click(1);
    }
    public void OnClick3()
    {      
        SoundManager.Instance.ButtonClick();

        SoundManager.Instance.OnBossStageStart();
        Stage1Click(2);
    }
    public void Stage1Click(int stage)
    {
        if(stage == 0)
        {
            Round.Instance.EntyUp();
            SoundManager.Instance.PlayGameBGM();
        }

        Round.Instance.RoundUp();

        // 현재 스테이지 목표 점수 설정
        Round.Instance.Score(stage);

        // 현재 스테이지 블라인드 이름 설정 
        Round.Instance.BlindName = Round.Instance.blindNames[stage];
        
        // 현재 스테이지 머니 갯수 설정 
        Round.Instance.Money = Round.Instance.moneys[stage];

        // 현재 스테이지 텍스트 설정
        Round.Instance.ScoreTextSetting(stage);

        // 현재 스테이지 색상 & 블라인드 이미지 설정
        Round.Instance.ImageSetting(stage);

        // 스테이지 플레이 시작
        GameManager.Instance.PlayOn();
        
        // 스테이지 창 비활성화
        stagecanvas.gameObject.SetActive(false);

        // 초기화
        CardManager.Instance.SetupNextStage();
    }



    // -------------------- 캐시 아웃 버튼 -----------------------

    public void NextEntyOn() // 캐시 아웃 버튼을 누르면 상점 나오게
    {
        SoundManager.Instance.ButtonClick();

        Entycanvas.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(true);
        SoundManager.Instance.OnShopStart();

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


    // 상점의 다음 라운드 버튼
    public void NextEnty()
    {
        SoundManager.Instance.ButtonClick();
        SoundManager.Instance.ResumePreviousBGM();

        OnShopCloseRequest?.Invoke();

        ShopPanel.gameObject.SetActive(false);
        stagecanvas.gameObject.SetActive(true);

    }

    public void OnEntyCanvas()
    {
        stagecanvas.gameObject.SetActive(true);
    }


}
