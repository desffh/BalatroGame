using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Round : Singleton<Round>
{
    // ��ü ���ھ� ������Ʈ
    [SerializeField] ScoreUISet scoreUiSet;

    // ū ��������
    [SerializeField] Queue<int> currentStages = new Queue<int>();

    // ���� ���� (���� ��������)
    [SerializeField] int currentStage;

    // ���� �������� ���� ����
    [SerializeField] int[] stageScores = new int[3];

    [SerializeField] TextMeshProUGUI [] blindScore;

    [SerializeField] int enty = 0;

    public int Enty
    {
        get { return enty; }
    }


    [SerializeField] int round = 0;

    [SerializeField] int [] stageMoney;

    public int[] stages
    {
        get { return stageScores; }
    }


    // ------- ���� �������� ��ǥ ���ھ� -------------------
    [SerializeField] int currentScores;

    public int CurrentScores
    {
        get { return currentScores; }
    }

    // ----------- Ȱ��ȭ �� �г� ---------------------------

    [SerializeField] GameObject[] stagepanels; 


    

    protected override void Awake()
    {
        base.Awake();

        // ť ���� ����
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

    // ���� �������� 3�� ����
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
    public void ScoreTextSetting()
    {
        scoreUiSet.EntyTextSetting("���� ����ε�", currentScores, "$$$");
    }

}
