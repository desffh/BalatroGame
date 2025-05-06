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

    [SerializeField] TextMeshProUGUI text;

    [SerializeField] TextMeshProUGUI shoptext;

    public static event System.Action OnShopCloseRequest;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        OnEntyCanvas();
        Typing(text);
    }

    public void OnClick1()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        Stage1Click(0);
    }
    public void OnClick2()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        Stage1Click(1);
    }
    public void OnClick3()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        ServiceLocator.Get<IAudioService>().PlayBGM("BossTheme-BossEnty", true);

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
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        ServiceLocator.Get<IAudioService>().PlayBGM("ShopTheme-Shop", true);

        Entycanvas.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(true);

        Typing(shoptext);

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
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        if (Round.Instance.Rounds % 3 == 0)
        {
            ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);   
        }
        else
        {
            ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
        }

        OnShopCloseRequest?.Invoke();

        ShopPanel.gameObject.SetActive(false);
        OnEntyCanvas();

    }

    public void OnEntyCanvas()
    {
        stagecanvas.gameObject.SetActive(true);
        Typing(text);
    }

    public void Typing(TextMeshProUGUI texts)
    {
        AnimationManager.Instance.TMProText(texts, 0.8f);
    }

}
