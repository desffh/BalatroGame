using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HoldManager : Singleton<HoldManager>
{
    // ��Ŀ�Ŵ��� �����ϱ�
    [SerializeField] PokerManager pokerManager;

    [SerializeField] GameOverPopUp gameOverPopUp;

    // ��ũ ���� ��ư ����
    [SerializeField] public Interactable interactable;



    // ��� �̺�Ʈ
    public UnityEvent calculation;

    public Action ActionSetting;

    WaitForSeconds waitForSeconds;

    // ���ڸ� ��� �ϳ��� ���� ���� ť
    private Queue<int> Num;

    [SerializeField] public int PlusSum;
    [SerializeField] public int MultiplySum;
    private int totalScore;

    [SerializeField] AnimationManager animationManager;

    private Queue<Func<IEnumerator>> actionQueue = new Queue<Func<IEnumerator>>();
    
    ShowRankText ShowRankText;

    ShowJokerRankText showJokerRankText;


    protected override void Awake()
    {
        base.Awake();
        ActionSetting += Setting;
    }

    private void Start()        
    {
        waitForSeconds = new WaitForSeconds(1.0f);

        PlusSum = 0;
        MultiplySum = 0;

        Num = new Queue<int>();

        RefillActionQueue();
    }


    public void RefillActionQueue()
    {
        actionQueue.Clear();  // ���� ť ����

        // ť�� �ʱ�ȭ�ϰų� �����ϰ�, �ʿ��� �۾��� �ٽ� �߰�
        //actionQueue.Enqueue(() => Setting());
        actionQueue.Enqueue(() => PlusCalculation()); // �� ���� ���ϱ�
        actionQueue.Enqueue(() => ApplyJokerEffectsStep());
        actionQueue.Enqueue(() => DelayedTotalScoreCal()); // ��ü ���ھ ����
        actionQueue.Enqueue(() => DelayedMove()); // �ϳ��� ���� ������ �̵�
        actionQueue.Enqueue(() => DelayActive()); // ��Ȱ��ȭ
    }

    public bool StageEnd()
    {
        if (totalScore < Round.Instance.CurrentScores && HandDelete.Instance.Hand <= 0)
        {
            Round.Instance.GameOverText();
            gameOverPopUp.GameOver();
            ScoreManager.Instance.BestHand();

            return true;
        }
        return false;
    }

    // �̺�Ʈ : 1. ť�� �� �� �ֱ� 2. ��� �� �ؽ�Ʈ ���� 3. ī�� ���� �� ��Ȱ��ȭ
    public void Calculation()
    {
        calculation.Invoke();
        //Debug.Log("��� ����");
    }

    // ��ϵ� �̺�Ʈ
    public void CalSetting()
    {
        var result = PokerManager.Instance.GetPokerResult();

        foreach (var n in result.SaveNum)
        {
            Num.Enqueue(n);
        }

        PlusSum = result.Plus;
        MultiplySum = result.Multiple;

        StartCoroutine(ExecuteActions());
    }


    private IEnumerator ExecuteActions()
    {

        while (actionQueue.Count > 0)
        {
            yield return StartCoroutine(actionQueue.Dequeue().Invoke());
        }

        if (ScoreManager.Instance.CheckStageClear(Round.Instance.CurrentScores) == false)
        {
            ActionSetting.Invoke();
        }
    }

    // �ٽ� ����
    public void Setting()
    {
        // ��� �� �ϰ� ����Ʈ �ʱ�ȭ
        pokerManager.ClearSelection();
        pokerManager.saveNum.Clear();

        UIupdate();

        CheckReset();


        // ���� ��Ƽ�� �� ��ٸ� ����
        StageEnd();
        

        if(StageEnd() == false)
        {
            // ī�� �Ѹ���, ���� ������, �ݶ��̴� �ѱ�
            CardManager.Instance.AddCardSpawn(() => {
                //CardManager.Instance.TurnOnAllCardColliders();
            });

            // �ٽ� �ݶ��̴� Ȱ��ȭ
            //CardManager.Instance.card.OnCollider();
            ButtonManager.Instance.ButtonInactive();

            // ���� ��ư Ȱ��ȭ
            interactable.OnButton();

            RefillActionQueue();
        }
    }

    // ����� ������ �� & �ٽ� ī�尡 ��ġ�Ǵ� ��
    IEnumerator DelayActive()
    {
        yield return waitForSeconds;
        pokerManager.DelaySetActive();
    }

    IEnumerator PlusCalculation()
    {
        while (true)
        {
            if(Num.Count <= 0)
            {
                yield break;
            }
            yield return waitForSeconds;

            Calculate();
        }
    }

    IEnumerator DelayedTotalScoreCal()
    {
        if (Num.Count == 0)
        {
            yield return new WaitForSeconds(1f);

            TotalScoreCal(); // ��Ŀ ȿ�� �ݿ��� MultiplySum/PlusSum ���
        }
    }


    private IEnumerator ApplyJokerEffectsStep()
    {
        Debug.Log("��Ŀ ȿ�� ���� �ܰ� ����");

        // **�� ������ ��� �� ���� �� ��ü�� �ʱ�ȭ �ð� Ȯ��**
        yield return null;

        var myJokerCard = FindAnyObjectByType<MyJokerCard>();

        // ��Ŀ�� ����Ʈ�� ���ٸ� �ڷ�ƾ ����
        if (myJokerCard == null || myJokerCard.Cards.Count == 0)
        {
            Debug.Log("��Ŀ�� �����Ƿ� ȿ�� ���� ���� ���� �ܰ�� ����");
            yield break;
        }

        yield return new WaitForSeconds(1f);

        List<Card> selectedCards = pokerManager.cardData.SelectCards.ToList();
        
        string currentHandType = PokerManager.Instance.pokerName;

        Debug.Log($"[��Ŀ ȿ��] ���� �ڵ�Ÿ��: {currentHandType}");
        Debug.Log($"[��Ŀ ȿ��] ���õ� ī�� ��: {selectedCards.Count}");

        foreach (var joker in myJokerCard.Cards)
        {
            Debug.Log($"[��Ŀ ȿ��] ��Ŀ �̸�: {joker.name}, ����: {joker.currentData.baseData.require}");

            joker.ActivateEffect(selectedCards, currentHandType, this, joker);

            showJokerRankText = joker.GetComponent<ShowJokerRankText>();
            showJokerRankText.OnSettingRank(joker.currentData.baseData.multiple);

            yield return new WaitForSeconds(1f);
        }


        Debug.Log("��Ŀ ���� �Ϸ�");
    }




    IEnumerator DelayedMove()
    {
        yield return waitForSeconds;
        pokerManager.DeleteMove();

        SoundManager.Instance.ResetSFXPitch();
    }
    

    // ---------------------------------------------


    IEnumerator deleteCard()
    {
        yield return waitForSeconds;

        pokerManager.DelaySetActive();

        // ����Ʈ �ʱ�ȭ
        pokerManager.ClearSelection();
        pokerManager.saveNum.Clear();

        // ī�� �Ѹ���, ���� ������, �ݶ��̴� �ѱ�
        CardManager.Instance.AddCardSpawn(() => {
            //CardManager.Instance.TurnOnAllCardColliders();
        });

        UIupdate();

        // �ٽ� �ݶ��̴� Ȱ��ȭ
        //CardManager.Instance.card.OnCollider();
        ButtonManager.Instance.ButtonInactive();

        // ���� ��ư Ȱ��ȭ
        interactable.OnButton();

        yield break;
    }

    // ---------------------------------------------


    public void TotalScoreCal()
    {

        totalScore = PlusSum * MultiplySum;

        ScoreManager.Instance.AddScore(totalScore);

        Debug.Log(totalScore); // �� ��ü ������ 0? 

        TextManager.Instance.UpdateText(0, ScoreManager.Instance.TotalScore);
    }
    

    public void UIupdate()
    {
        TextManager.Instance.stringUpdateText(0);

        TextManager.Instance.UpdateText(1);
        TextManager.Instance.UpdateText(2);

        //textManager.BufferUpdate();
    }
    public void TotalScoreupdate()
    {
        TextManager.Instance.UpdateText(0);
    }

    
    public void Calculate()
    {
        if (Num.Count > 0)
        {
            // ť���� ���鼭 üũ
            int saveNumber = Num.Dequeue();
            PlusSum += saveNumber;

            // �ִϸ��̼� ȣ��
            animationManager.PlayCardAnime(SaveNumber(saveNumber));

            TextManager.Instance.UpdateText(1, PlusSum);
            
            // �ִϸ��̼��� ������ �������� �ؽ�Ʈ�� ������Ʈ ���� �ʱ� ������ �ؽ�Ʈ ������Ʈ ���Ŀ� ���� ����
            if (animationManager.moveTween != null && animationManager.moveTween.IsPlaying()) return;

        }
     
    }

    // �ִϸ��̼��� ȣ���ϱ� ���� ���
    private GameObject game;

    private bool[] savenumberCheck = new bool[5];
    public GameObject SaveNumber(int saveNumber)
    {
        for (int i = 0; i < pokerManager.cardData.SelectCards.Count; i++)
        {
            if (savenumberCheck[i] == false && pokerManager.cardData.SelectCards[i].itemdata.id == saveNumber)
            {
                game = pokerManager.cardData.SelectCards[i].gameObject;

                savenumberCheck[i] = true;

                ShowRankText = PokerManager.Instance.cardData.SelectCards[i].GetComponent<ShowRankText>();
                ShowRankText.OnSettingRank(saveNumber);

                // ���⼭ ȿ���� ����ϰ� ��ġ �ø���
                SoundManager.Instance.sfxSource.pitch += 0.04f;
                SoundManager.Instance.PlayCardCountSFX();  // ���� �Լ��� ����

                break;
            }
        }
        return game;
    }
    
    

    // �ִϸ��̼� �Ǵܿ��� bool�迭 �ʱ�ȭ
    public void CheckReset()
    {
        for(int i = 0; i < 5; i++)
        {
            savenumberCheck[i] = false;
        }
    }

    // ���� �� ����
    public void PokerCalculate(int plus, int multiple)
    {
        if (pokerManager.cardData.SelectCards.Count > 0)
        {
            PlusSum += plus;
            
            MultiplySum += multiple;
        }
        //textManager.PokerUpdate(PlusSum, MultiplySum);
    }

    public void StartDeleteCard()
    {
        // ������ ���� ī���� �ݶ��̴� ��Ȱ��ȭ               
        //CardManager.Instance.card.OffCollider();

        interactable.OffButton();

        // ī�� ��Ȱ��ȭ & �ݶ��̴� Ȱ��ȭ 
        StartCoroutine(deleteCard());
    }
}
