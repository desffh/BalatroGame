using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������� ���� & ����,��Ƽ �������� (MVP)

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


    // �� entyStage���� -> BlindRound (�Ϲ�2 + ����1)
    //
    // �� ���� ������ ���� BlindRound
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

        Debug.Log($"Awake() ���� - entyStages.Count: {entyStages?.Count}");

        SetEntyStages();
    }

    private void Start()
    {

        GenerateAllBlindRounds();

        Reset();
        
    }

    // baseScores�� ���� ����Ʈ�� �⺻ ���� ���� (���� ����ε�)
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


    // ����ε� 3�� ���� ���� (�̸�, ��ǥ ����, �Ӵ�, �̹���, ����)
    private void GenerateAllBlindRounds()
    {
        blindRoundsPerEnty.Clear();

        for (int i = 0; i < entyStages.Count; i++)
        {
            var enty = entyStages[i];
            var boss = bossDataReader.DataList[i];

            var group = new List<BlindRound>();

            // �Ϲ� ����ε� 2�� (���� ����ε�, �� ����ε�)
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

            // ���� ����ε�
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

            // BlindRound�� ��� ����Ʈ�� �߰�
            blindRoundsPerEnty.Add(group);
        }
    }

    // blindRoundsPerEnty[0] = [ BlindRound0-1, BlindRound0-2, BossRound0 ]
    // blindRoundsPerEnty[1] = [ BlindRound1-1, BlindRound1-2, BossRound1 ]

    // [���� ��Ƽ] [���� ����ε�]
    public BlindRound GetBlindAtCurrentEnty(int blindIndex)
    {
        if (currentEntyIndex >= blindRoundsPerEnty.Count)
        {
            Debug.LogError($"[StageManager] �߸��� currentEntyIndex: {currentEntyIndex}");
            return null;
        }

        var blindList = blindRoundsPerEnty[currentEntyIndex];

        if (blindIndex < 0 || blindIndex >= blindList.Count)
        {
            Debug.LogError($"[StageManager] blindIndex({blindIndex})�� ������ ��� (0~{blindList.Count - 1})");
            return null;
        }

        return blindList[blindIndex];
    }

    // ���� ��Ƽ ��ȯ
    public EntyStage Getblind()
    {
        return entyStages[currentEntyIndex];
    }


    // ���� ����ε� �̵� (0 1 2)
    public void AdvanceBlind()
    {
        currentBlindIndex++;
        if (currentBlindIndex >= 3)
        {
            // �ٽ� 0����ε� ���� ����
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
