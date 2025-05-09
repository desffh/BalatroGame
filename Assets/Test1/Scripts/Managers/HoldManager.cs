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
    // 포커매니저 참조하기
    [SerializeField] PokerManager pokerManager;

    [SerializeField] GameOverPopUp gameOverPopUp;

    // 랭크 정렬 버튼 참조
    [SerializeField] public Interactable interactable;

    // 계산 이벤트
    public UnityEvent calculation;

    public Action ActionSetting;

    WaitForSeconds waitForSeconds;

    // 숫자를 담고 하나씩 빼기 위한 큐
    private Queue<int> Num;

    [SerializeField] AnimationManager animationManager;

    private Queue<Func<IEnumerator>> actionQueue = new Queue<Func<IEnumerator>>();
    
    ShowRankText ShowRankText;

    ShowJokerRankText showJokerRankText;

    MyJokerCard myJokerCard;

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
        actionQueue.Clear();  // 기존 큐 비우기

        // 큐를 초기화하거나 리셋하고, 필요한 작업을 다시 추가
        //actionQueue.Enqueue(() => Setting());
        actionQueue.Enqueue(() => PlusCalculation()); // 각 값을 더하기
        actionQueue.Enqueue(() => ApplyJokerEffectsStep());
        actionQueue.Enqueue(() => DelayedTotalScoreCal()); // 전체 스코어에 갱신
        actionQueue.Enqueue(() => DelayedMove()); // 하나씩 삭제 존으로 이동
        actionQueue.Enqueue(() => DelayActive()); // 비활성화
    }

    public bool StageEnd()
    {
        if (ScoreManager.Instance.CurrentScore < Round.Instance.CurrentScores && StateManager.Instance.HandDelete.Hand <= 0)
        {
            Round.Instance.GameOverText();
            gameOverPopUp.GameOver();
            ScoreManager.Instance.BestHand();

            return true;
        }
        return false;
    }

    // 이벤트 : 1. 큐에 값 다 넣기 2. 계산 후 텍스트 누적 3. 카드 날라간 뒤 비활성화
    public void Calculation()
    {
        
        calculation.Invoke();
        //Debug.Log("계산 시작");
    }

    // 등록된 이벤트
    public void CalSetting()
    {
        myJokerCard = FindAnyObjectByType<MyJokerCard>();

        foreach (var joker in myJokerCard.Cards) // 계산 시작 시 
        {
            joker.isCalculating = true;
            Debug.Log("조커카드 클릭 안돼요");
        }
        
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

        if (ScoreManager.Instance.CheckStageClear(Round.Instance.CurrentScores) == false)
        {
            ActionSetting.Invoke();
        }
    }

    // 다시 셋팅
    public void Setting()
    {
        // 계산 다 하고 리스트 초기화
        pokerManager.ClearSelection();
        pokerManager.saveNum.Clear();

        UIupdate();

        CheckReset();


        // 만약 엔티를 다 썼다면 종료
        StageEnd();
        

        if(StageEnd() == false)
        {
            // 카드 뿌리고, 정렬 끝나고, 콜라이더 켜기
            CardManager.Instance.AddCardSpawn(() => {
                //CardManager.Instance.TurnOnAllCardColliders();
            });

            // 다시 콜라이더 활성화
            //CardManager.Instance.card.OnCollider();
            ButtonManager.Instance.ButtonInactive();

            // 정렬 버튼 활성화
            interactable.OnButton();

            RefillActionQueue();


        }
    }

    // 계산이 끝나는 곳 & 다시 카드가 배치되는 곳
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

            TotalScoreCal(); // 조커 효과 반영된 MultiplySum/PlusSum 사용
        }
    }


    private IEnumerator ApplyJokerEffectsStep()
    {

        // **한 프레임 대기 후 실행 → 객체들 초기화 시간 확보**
        yield return null;


        // 조커가 리스트에 없다면 코루틴 종료
        if (myJokerCard == null || myJokerCard.Cards.Count == 0)
        {

            yield break;
        }

        yield return new WaitForSeconds(1f);

        List<Card> selectedCards = pokerManager.cardData.SelectCards.ToList();
        
        string currentHandType = PokerManager.Instance.pokerName;


        foreach (var joker in myJokerCard.Cards)
        {
            Debug.Log($"[조커 효과] 조커 이름: {joker.name}, 조건: {joker.currentData.baseData.require}");

            bool effectApplied = joker.ActivateEffect(selectedCards, currentHandType, StateManager.Instance, joker);

            if (effectApplied)
            {
                yield return new WaitForSeconds(1f);
            }
        }

        foreach (var joker in myJokerCard.Cards)
        {
            joker.isCalculating = false;
            Debug.Log("조커카드 클릭돼요");
        }

        Debug.Log("조커 루프 완료");
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

        // 리스트 초기화
        pokerManager.ClearSelection();
        pokerManager.saveNum.Clear();

        CardManager.Instance.AddCardSpawn(() => {
        });

        UIupdate();

        // 다시 콜라이더 활성화
        //CardManager.Instance.card.OnCollider();
        ButtonManager.Instance.ButtonInactive();

        // 정렬 버튼 활성화
        interactable.OnButton();

        yield break;
    }

    // ---------------------------------------------


    public void TotalScoreCal()
    {
        int lastScore = ScoreManager.Instance.TotalScore;

        ScoreManager.Instance.AddScore(StateManager.Instance.MultipleChip.MULTIPLYSum * StateManager.Instance.MultipleChip.PLUSSum);

        ScoreManager.Instance.AnimateScore(lastScore, ScoreManager.Instance.TotalScore);
        
        Debug.Log($"이전 점수 : {lastScore} , 현재 점수 : {ScoreManager.Instance.TotalScore}");
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
            // 큐에서 빼면서 체크
            int saveNumber = Num.Dequeue();
            StateManager.Instance.MultipleChip.PlusPlusSum(saveNumber);

            // 애니메이션 호출
            animationManager.PlayCardAnime(SaveNumber(saveNumber));

            //TextManager.Instance.UpdateText(1, StateManager.Instance.MultipleChip.PLUSSum);
            
            // 애니메이션이 끝나기 전까지는 텍스트가 업데이트 되지 않기 때문에 텍스트 업데이트 이후에 로직 설정
            if (animationManager.moveTween != null && animationManager.moveTween.IsPlaying()) return;

        }
     
    }

    // 애니메이션을 호출하기 위해 사용
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

                ServiceLocator.Get<IAudioServicePitch>().PlaySFXPitch("Sound-CheckCard");

                break;
            }
        }
        return game;
    }
    
    

    // 애니메이션 판단여부 bool배열 초기화
    public void CheckReset()
    {
        for(int i = 0; i < 5; i++)
        {
            savenumberCheck[i] = false;
        }
    }

    public void StartDeleteCard()
    {
        // 버리는 동안 카드의 콜라이더 비활성화               
        //CardManager.Instance.card.OffCollider();

        interactable.OffButton();

        // 카드 비활성화 & 콜라이더 활성화 
        StartCoroutine(deleteCard());
    }
}
