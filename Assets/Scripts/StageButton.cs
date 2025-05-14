using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


// 블라인드 선택 버튼을 누를 때 실행되는 함수들

public class StageButton : MonoBehaviour
{
    [SerializeField] private Button[] blindButtons; // 총 3개

    [SerializeField] private TextMeshProUGUI [] scoreText; // 목표점수 텍스트

    [SerializeField] private StageManager stageManager;

    [SerializeField] private ScoreUISet scoreUiSet;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private TextMeshProUGUI shoptext;

    [SerializeField] GameObject Entycanvas; // 결과창 캔버스

    [SerializeField] private Canvas stagecanvas;

    [SerializeField] private GameObject ShopPanel;

    [SerializeField] Button nextEnty; // 다음 엔티

    // 상점이 닫힐 때 호출되는 대리자
    public static event System.Action OnShopCloseRequest;

    // 매 라운드가 시작될 때 마다 호출되는 대리자
    //
    // -> 라운드마다 초기화를 필요로 하는 조커에 함수 발동
    public static event System.Action OnRoundStart;

    // ----------- 활성화 할 패널 ---------------------------

    [SerializeField] GameObject[] stagepanels;

    public BlindRound blind;


    [SerializeField] private Image bossBlindImage;

    [SerializeField] private TextMeshProUGUI bossInfoText;

    //---



    private void Start()
    {
        OnEntyCanvas();
        Typing(text);

        // 블라인드 패널에 첫 스코어 셋팅
        ScoreTextSetting(); 
        PanelSetting();

        // |---

        // 보스 블라인드 이미지 설정
       
        BossImageSetting();


        // 보스 블라인드 텍스트 설정

        BossInfoSetting();
    }

    public void OnBlindClick0()
    {
        // 엔티 증가
        StageManager.Instance.EntyAdd();
        OnBlindClick(0);

    }
    public void OnBlindClick1() => OnBlindClick(1);
    
    
    // 보스 블라인드
    public void OnBlindClick2()
    {
        OnBlindClick(2);

    }

    public void OnBlindClick(int blindIndex)
    {
        // 라운드 증가
        StageManager.Instance.RoundAdd();

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        blind = stageManager.GetBlindAtCurrentEnty(blindIndex);


        Debug.Log(blind.blindColor);

        scoreUiSet.EntyTextSetting(blind.blindName, blind.score, new string('$', blind.money));
        scoreUiSet.EntyImageSetting(blind.blindImage, blind.blindColor);

        if (blind.isBoss)
        {
            ServiceLocator.Get<IAudioService>().PlayBGM("BossTheme-BossEnty", true);
            
        }

        Panels(blindIndex);

        GameManager.Instance.PlayOn();
        stagecanvas.gameObject.SetActive(false);

        CardManager.Instance.SetupNextStage();

        // 매 라운드가 시작될 때 마다 호출되는 대리자
        //
        // -> 라운드마다 초기화를 필요로 하는 조커에 함수 발동
        OnRoundStart?.Invoke();
    }

    // 가림막 패널 활성화
    public void Panels(int stage)
    {
        stagepanels[stage].SetActive(true);

        stagepanels[(stage + 1) % 3].SetActive(false);
    }

    // 가림판 패널 초기 셋팅
    public void PanelSetting()
    {
        for (int i = 0; i < stagepanels.Length; i++)
        {
            stagepanels[i].SetActive(true);
        }

        stagepanels[0].SetActive(false);
    }

    public void OnEntyCanvas()
    {
        stagecanvas.gameObject.SetActive(true);
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-Enty");

        Typing(text);

        // 엔티 활성화 될 때 보스 이미지 설정
        BossImageSetting();


        // 보스 블라인드 텍스트 설정

        BossInfoSetting();
    }

    private void Typing(TextMeshProUGUI text)
    {
        AnimationManager.Instance.TMProText(text, 0.8f);

        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-BlindText");

    }

    // -------------------- 캐시 아웃 버튼 -----------------------

    public void NextEntyOn() // 캐시 아웃 버튼을 누르면 상점 나오게
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        ServiceLocator.Get<IAudioService>().PlayBGM("ShopTheme-Shop", true);

        Entycanvas.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(true);

        Typing(shoptext);

        // 목표 점수 다시 셋팅
        stageManager.AdvanceBlind();
    }

    public void NextEntyOff()
    {
        nextEnty.interactable = false;
    }


    // 상점의 다음 라운드 버튼
    public void NextEnty()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        // 다음 라운드가 스몰 블라인드라면 
        if (stageManager.GetRound() % 3 == 0)
        {
            ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
            
            // 다음 엔티 갈 때 
            ScoreTextSetting();


        }


        OnShopCloseRequest?.Invoke();

        ShopPanel.gameObject.SetActive(false);

        // |---



        // 블라인드 캔버스 열기 
        OnEntyCanvas();
    }

    // 엔티의 보스 블라인드 이미지 설정
    public void BossImageSetting()
    {
        BlindRound blindInfo =  stageManager.BossBlindInfo(2);

        bossBlindImage.sprite = blindInfo.blindImage;
    }

    public void BossInfoSetting()
    {
        BlindRound blindInfo = stageManager.BossBlindInfo(2);

        bossInfoText.text = blindInfo.blindInfo;
    }


    // 게임 오버 팝업 업데이트|-------------------------------------

    [SerializeField] GameOverText gameOverText;

    public void GameOverText()
    {
        gameOverText.EntyUpdate(StageManager.Instance.GetEnty());
        gameOverText.RoundUpdate(StageManager.Instance.GetRound());
        gameOverText.BlindUpdate(blind.blindName, blind.blindImage);
    }

    // 라운드, 엔티 횟수 리셋 |----------------------------------------------


    public void ScoreTextSetting()
    {
        EntyStage entys = stageManager.Getblind();

        for (int i = 0; i < entys.blindScore.Length; i++)
        {
            scoreText[i].text = entys.blindScore[i].ToString();
        }
    }

    public void ReSettings()
    {
        // 스코어 셋팅 다시하기
        ScoreTextSetting();

        // 가림판 초기 셋팅 
        PanelSetting();

        // 엔티 캔버스 활성화
        OnEntyCanvas();


    }
}
