using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class Round : Singleton<Round>
{
    // 전체 스코어 컴포넌트
    [SerializeField] ScoreUISet scoreUiSet;

    // 큰 스테이지
    [SerializeField] Queue<int> currentStages = new Queue<int>();

    // 현재 라운드 (작은 스테이지)
    [SerializeField] int currentStage;

    // 현재 스테이지 점수 셋팅
    [SerializeField] int[] stageScores = new int[3];

    [SerializeField] TextMeshProUGUI [] blindScore;

    [SerializeField] int enty = 0;

    public int Enty
    {
        get { return enty; }
    }


    [SerializeField] int round = 0;

    public int Rounds
    {
        get { return round; }
    }

    [SerializeField] int [] stageMoney;

    public int[] stages
    {
        get { return stageScores; }
    }


    // ------- 현재 스테이지 목표 스코어 -------------------
    [SerializeField] int currentScores;

    public int CurrentScores
    {
        get { return currentScores; }
    }

    // ----------- 활성화 할 패널 ---------------------------

    [SerializeField] GameObject[] stagepanels;


    // |---------------------------------------------------
    // 각 스테이지 별 획득 머니 수
    [SerializeField] public int[] moneys;

    // 각 스테이지 별 블라인드 이름
    [SerializeField] public string[] blindNames;

    // 각 스테이지 별 블라인드 색상
    [SerializeField] public UnityEngine.Color[] moneyNames;

    // 각 스테이지 별 블라인드 이미지
    [SerializeField] public Sprite [] blindImages;

    // |---------------------------------------------------

    [SerializeField] GameOverText gameOverText;

    private int money;

    public int Money
    {
        get { return money; }
        set { money = value; }
    }


    private string blindName;

    public string BlindName
    {
        get { return blindName; }
        set { blindName = value; }
    }

    public Sprite blindimage;



    protected override void Awake()
    {
        base.Awake();

        // 큐 점수 셋팅
        currentStages.Enqueue(300); // 300 450 600
        currentStages.Enqueue(800); // 800 1200 1600
        currentStages.Enqueue(1800);// 1800 2700 3600
        
        ScoreSetting();

        for (int i = 0; i < stagepanels.Length; i++)
        {
            stagepanels[i].SetActive(true);
        }

        stagepanels[0].SetActive(false);

        for(int i = 0; i < stageMoney.Length; i++)
        {
            stageMoney[i] = i + 3;
        }

    }

    private bool isStageUpdated = false;

    // 작은 스테이지 3개 셋팅
    public void ScoreSetting()
    {
        if (isStageUpdated) return;

        OnEnty();

        if (currentStages.Count > 0)
        {
            currentStage = currentStages.Dequeue();

            for (int i = 0; i < stageScores.Length; i++)
            {
                stageScores[i] = currentStage + (i * currentStage / 2);
                blindScore[i].text = stageScores[i].ToString();  
            }
        }

        isStageUpdated = true; 
    }

    public void Score(int stage)
    {
        currentScores = stageScores[stage];

        stagepanels[stage].SetActive(true);

        stagepanels[(stage + 1) % 3].SetActive(false);

        OnEnty();
        OnRound();
    }

    public void OnEnty()
    {
        TextManager.Instance.UpdateText(5, enty);
    }

    public void EntyUp()
    {
        enty++;
    }
    public void OnRound()
    {
        TextManager.Instance.UpdateText(6, round);
    }

    public void RoundUp()
    {
        round++;
    }
    public void isStage()
    {
        isStageUpdated = false;
    }

    // |-------------------------------------
    public void ScoreTextSetting(int count)
    {
        blindName = blindNames[count];

        // 아니 이런 실수를... count는 인덱스 값이야 ㅠㅠㅠ
        Money = moneys[count];

        string moneytexts = new string('$', Money); // '$'를 count만큼 반복

        scoreUiSet.EntyTextSetting(blindName, currentScores, moneytexts);
    }

    public void ImageSetting(int count)
    {
        blindimage = blindImages[count];

        scoreUiSet.EntyImageSetting(blindimage, moneyNames[count]);
    }

    // 게임 오버 팝업 업데이트|-------------------------------------

    public void GameOverText()
    {
        gameOverText.EntyUpdate(Enty);
        gameOverText.RoundUpdate(Rounds);
        gameOverText.BlindUpdate(BlindName, blindimage);
    }
}
