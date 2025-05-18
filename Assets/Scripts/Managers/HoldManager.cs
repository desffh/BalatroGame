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

    [SerializeField] AnimationManager animationManager;

    private Queue<Func<IEnumerator>> actionQueue = new Queue<Func<IEnumerator>>();
    
    ShowRankText ShowRankText;

    ShowJokerRankText showJokerRankText;

    MyJokerCard myJokerCard;

    [SerializeField] StageButton stageButton;

    protected override void Awake()
    {
        base.Awake();
        ActionSetting += Setting;
    }

    private void Start()        
    {
        waitForSeconds = new WaitForSeconds(1.0f);

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
        int returnhands = StateManager.Instance.handDeleteSetting.GetHand();

        if (ScoreManager.Instance.CurrentScore < stageButton.blind.score && returnhands <= 0)
        {
            stageButton.GameOverText();
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
        myJokerCard = FindAnyObjectByType<MyJokerCard>();

        foreach (var joker in myJokerCard.Cards) // ��� ���� �� 
        {
            joker.isCalculating = true;
            Debug.Log("��Ŀī�� Ŭ�� �ȵſ�");
        }

        // Ÿ��ī�嵵 �߰��ؾ���

        
        var result = PokerManager.Instance.GetPokerResult();

        foreach (var n in result.SaveNum)
        {
            Num.Enqueue(n);
        }

        StartCoroutine(ExecuteActions());
    }


    private IEnumerator ExecuteActions()
    {

        while (actionQueue.Count > 0)
        {
            yield return StartCoroutine(actionQueue.Dequeue().Invoke());
        }

        if (ScoreManager.Instance.CheckStageClear(stageButton.blind.score) == false)
        {
            //Debug.Log(stageButton.blind.score);

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

        // **�� ������ ��� �� ���� �� ��ü�� �ʱ�ȭ �ð� Ȯ��**
        yield return null;


        // ��Ŀ�� ����Ʈ�� ���ٸ� �ڷ�ƾ ����
        if (myJokerCard == null || myJokerCard.Cards.Count == 0)
        {

            yield break;
        }

        yield return new WaitForSeconds(1f);

        List<Card> selectedCards = pokerManager.cardData.SelectCards.ToList();

        // ���� ������ ���� ���� ��������
        List<string> handTypes = PokerManager.Instance.GetContainedHandTypes();

        foreach (var joker in myJokerCard.Cards)
        {
            Debug.Log($"[��Ŀ ȿ��] ��Ŀ �̸�: {joker.name}, ����: {joker.currentData.baseData.require}");

            var context = new JokerEffectContext
            {
                StateManager = StateManager.Instance,
                MyJoker = joker,
                MyJokerCard = myJokerCard,
                HandTypes = handTypes,
                SelectedCards = selectedCards
            };
            var effect = joker.GetEffect(); // IJokerEffect Ÿ��

            bool effectApplied = joker.ActivateEffect(context);


            if (effectApplied)
            {
                // �ִϸ��̼� �������� ������ �����ͼ� ��ٸ�
                if (effect is JokerCardEffect concreteEffect)
                {
                    var seq = concreteEffect.GetAnimationSequence();

                    if (seq != null)
                    {
                        yield return seq.WaitForCompletion(); // �ִϸ��̼��� ��� ���� ������ ���
                    }
                }
                else
                {
                    yield return new WaitForSeconds(1f); // fallback
                }
            }
        }

        foreach (var joker in myJokerCard.Cards)
        {
            joker.isCalculating = false;
            Debug.Log("��Ŀī�� Ŭ���ſ�");
        }

        Debug.Log("��Ŀ ���� �Ϸ�");
    }

    IEnumerator DelayedMove()
    {
        yield return waitForSeconds;
        pokerManager.DeleteMove();


    }
    

    // ---------------------------------------------


    IEnumerator deleteCard()
    {
        yield return waitForSeconds;

        pokerManager.DelaySetActive();

        // ����Ʈ �ʱ�ȭ
        pokerManager.ClearSelection();
        pokerManager.saveNum.Clear();

        CardManager.Instance.AddCardSpawn(() => {
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
        int lastScore = ScoreManager.Instance.TotalScore;


        int multiple = StateManager.Instance.multiplyChipSetting.GetMultiply();
        int plus = StateManager.Instance.multiplyChipSetting.GetChip();

        ScoreManager.Instance.AddScore(multiple * plus);

        ScoreManager.Instance.AnimateScore(lastScore, ScoreManager.Instance.TotalScore);
        
        Debug.Log($"���� ���� : {lastScore} , ���� ���� : {ScoreManager.Instance.TotalScore}");
    }
    

    public void UIupdate()
    {
        TextManager.Instance.stringUpdateText(0);

        StateManager.Instance.multiplyChipSetting.Reset();


        //textManager.BufferUpdate();
    }
    public void TotalScoreupdate()
    {
        TextManager.Instance.UpdateText(0);
    }



    // ����� �����ϱ�
    public int ApplyBossDebuff(int saveNumber)
    {
        // ���� ����ε� ����
        int blindIndex = StageManager.Instance.GetCurrentBlindIndex();

        BlindRound currentDebuff = StageManager.Instance.GetBlindAtCurrentEnty(blindIndex);


        // null�̰�, ������ �ƴ϶�� ���� itemdata.id ��ȯ
        if (currentDebuff == null || !currentDebuff.isBoss || currentDebuff.bossDebuff == null)
            return saveNumber;

        // ī�� ������� ó�� -> ī�� ������� �ƴ϶�� ��ȯ
        if (currentDebuff.bossDebuff is not ICardDebuff cardDebuff)
            return saveNumber;

        //// ������ �ƴ϶�� null ��
        //IBossDebuff bossDebuff = currentDebuff.bossDebuff;  

        var selectedCards = pokerManager.cardData.SelectCards;

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var card = selectedCards[i];

            // ���� ó������ ���� ī�� �߿��� saveNumber�� ������ id�� ���� ī��
            if (!savenumberCheck[i] && card.itemdata.id == saveNumber)
            {
                Debug.Log("���� ����Ǿ����");
                // �� ī�忡 ����� ���� ���� Ȯ��
                return cardDebuff.ApplyDebuff(card) ? 0 : saveNumber;
            }
        }

        // �� ã������ �״��
        return saveNumber;
    }

    // Ÿ�� �����ϱ�
    public int ApplyTaroEffectScore(int baseId)
    {
        var selectedCards = pokerManager.cardData.SelectCards;

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var card = selectedCards[i];

            if (!savenumberCheck_Taro[i] && card.itemdata.id == baseId)
            {
                foreach (var effect in PokerManager.Instance.GetActiveTaroEffects())
                {
                    if (card.itemdata.suit == effect.requireSuit &&
                        card.itemdata.id == effect.requireNumber)
                    {
                        savenumberCheck_Taro[i] = true;
                        return baseId + card.EffectiveId;
                    }
                }

                // Ÿ�� ȿ�� ������ üũ�� �ɷ� ó��
                savenumberCheck_Taro[i] = true;
                return baseId;
            }
        }

        return baseId;
    }




    public void Calculate()
    {
        if (Num.Count > 0)
        {
            // ť���� ���鼭 üũ
            int saveNumber = Num.Dequeue();


            // 1. ����� ���� ����
            int debuffedScore = ApplyBossDebuff(saveNumber);  // ����� 0

            // 2. ������� �ƴ� ��쿡�� Ÿ�� ȿ�� ����
            int finalScore = (debuffedScore > 0) ? ApplyTaroEffectScore(debuffedScore) : 0;

            // �� ���ϱ� (MVP)
            StateManager.Instance.multiplyChipSetting.AddPlus(finalScore);
            
            // �ִϸ��̼� ȣ��
            animationManager.PlayCardAnime(SaveNumber(saveNumber, finalScore));
        }
     
    }


    // �ִϸ��̼��� ȣ���ϱ� ���� ��� -> ������� ���� saveNumber�� 0�� �Ǿ��� ��� �ִϸ��̼��� ������� ���� & 0������ �ִϸ��̼��� �� -> ó������ ������
    //
    // -> X �ε��� ������� ó�� (������ ���� �� ī����� ������������ ���ĵ�)
    //
    // -> �ȵ�. �ε��� ������� ó���ϸ� saveNum�� ���� ���� �ֵ鵵 �ִϸ��̼� ����
    private GameObject game;

    private bool[] savenumberCheck = new bool[5]; // �������
    private bool[] savenumberCheck_Taro = new bool[5];   // Ÿ�ο�

    // �Ű������� saveNum�� �� ����
    public GameObject SaveNumber(int saveNumber, int finalScore)
    {
        var selectedCards = pokerManager.cardData.SelectCards;

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var card = selectedCards[i];

            if (!savenumberCheck[i] && card.itemdata.id == saveNumber)
            {
                savenumberCheck[i] = true;
                game = card.gameObject;

                ShowRankText = card.GetComponent<ShowRankText>();
                ShowRankText.OnSettingRank(finalScore);  // ���� ���� ���� ǥ��

                ServiceLocator.Get<IAudioServicePitch>().PlaySFXPitch("Sound-CheckCard");
                return game;
            }
        }

        return null;
    }







    // �ִϸ��̼� �Ǵܿ��� bool�迭 �ʱ�ȭ
    public void CheckReset()
    {
        for(int i = 0; i < 5; i++)
        {
            savenumberCheck[i] = false;
            savenumberCheck_Taro[i] = false;
        }
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
