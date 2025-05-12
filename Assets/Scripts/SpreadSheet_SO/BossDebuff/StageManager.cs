using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지 셋팅 & 라운드,엔티 프레젠터 (MVP)

public class StageManager : Singleton<StageManager>, IRoundEntySetting
{

    [SerializeField] private RoundEntyData data;
    [SerializeField] private RoundEntyView view;

    //---

    [SerializeField] private List<EntyStage> entyStages;

    [SerializeField] private BossDataReader bossDataReader;

    //[SerializeField] private List<BossData> bossData;

    [SerializeField] private int currentEntyIndex = 0;

    [SerializeField] private int currentBlindIndex = 0;


    // 각 entyStage마다 -> BlindRound (일반2 + 보스1)
    //
    // 각 라운드 정보를 담은 BlindRound
    private List<List<BlindRound>> blindRoundsPerEnty = new();

    private int [] baseScores = { 300, 800, 1800, 3800, 7800 };


    public void Init(RoundEntyData data, RoundEntyView view)
    {
        this.data = data;
        this.view = view;

        view.UpdateEnty(data.Enty);
        view.UpdateRound(data.Round);
    }


    protected override void Awake()
    {
        base.Awake();

        Init(data, view); 

        Debug.Log($"Awake() 시작 - entyStages.Count: {entyStages?.Count}");

        SetEntyStages();
    }

    private void Start()
    {

        GenerateAllBlindRounds();

        Reset();
        
    }

    // baseScores를 토대로 리스트의 기본 점수 셋팅 (스몰 블라인드)
    private void SetEntyStages()
    {
        for (int i = 0; i < baseScores.Length; i++)
        {
            if (i < entyStages.Count)
            {
                entyStages[i].baseScore = baseScores[i];

                entyStages[i].GetBlind();
            }
        }
        
    }


    // 블라인드 3개 정보 셋팅 (이름, 목표 점수, 머니, 이미지, 색상)
    private void GenerateAllBlindRounds()
    {
        blindRoundsPerEnty.Clear();

        for (int i = 0; i < entyStages.Count; i++)
        {
            var enty = entyStages[i];
            var boss = bossDataReader.DataList[i];

            var group = new List<BlindRound>();

            // 일반 블라인드 2개 (스몰 블라인드, 빅 블라인드)
            for (int j = 0; j < 2; j++)
            {
                group.Add(new BlindRound
                {
                    blindName = enty.normalBlindNames[j],
                    score = enty.GetBlindScore(j),
                    money = enty.normalMoneys[j],
                    blindImage = enty.normalBlindImages[j],
                    blindColor = enty.normalColors[j],
                    isBoss = false,
                    bossDebuff = null
                });
            }

            // 보스 블라인드
            group.Add(new BlindRound
            {
                blindName = boss.blindname,
                score = enty.GetBlindScore(2),
                money = boss.money,
                blindImage = Resources.Load<Sprite>($"Blind/{boss.blindImage}"),
                blindColor = enty.normalColors[2],
                isBoss = true,
                //bossDebuff = BossDebuffFactory.Create(boss)
            });

            // BlindRound를 담는 리스트에 추가
            blindRoundsPerEnty.Add(group);
        }
    }

    // blindRoundsPerEnty[0] = [ BlindRound0-1, BlindRound0-2, BossRound0 ]
    // blindRoundsPerEnty[1] = [ BlindRound1-1, BlindRound1-2, BossRound1 ]

    // [현재 엔티] [현재 블라인드]
    public BlindRound GetBlindAtCurrentEnty(int blindIndex)
    {
        if (currentEntyIndex >= blindRoundsPerEnty.Count)
        {
            Debug.LogError($"[StageManager] 잘못된 currentEntyIndex: {currentEntyIndex}");
            return null;
        }

        var blindList = blindRoundsPerEnty[currentEntyIndex];

        if (blindIndex < 0 || blindIndex >= blindList.Count)
        {
            Debug.LogError($"[StageManager] blindIndex({blindIndex})가 범위를 벗어남 (0~{blindList.Count - 1})");
            return null;
        }

        return blindList[blindIndex];
    }

    // 현재 엔티 반환
    public EntyStage Getblind()
    {
        return entyStages[currentEntyIndex];
    }


    // 현재 블라인드 이동 (0 1 2)
    public void AdvanceBlind()
    {
        currentBlindIndex++;
        if (currentBlindIndex >= 3)
        {
            // 다시 0블라인드 부터 시작
            currentBlindIndex = 0;
            currentEntyIndex++;
        }
    }

    public bool IsBossBlind(int blindIndex)
    {
        return blindRoundsPerEnty[currentEntyIndex][blindIndex].isBoss;
    }

    public void EntyAdd()
    {
        data.UpEnty();
        view.UpdateEnty(data.Enty);
    }

    public void RoundAdd()
    {
        data.UpRound();
        view.UpdateRound(data.Round);
    }

    public void Reset()
    {
        data.ResetCounts();

        view.UpdateEnty(data.Enty);
        view.UpdateRound(data.Round);
    }

    public int GetEnty()
    {
        return data.Enty;
    }

    public int GetRound()
    {
        return data.Round;
    }
}
