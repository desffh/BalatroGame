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

        if (ScoreManager.Instance.CheckStageClear(stageButton.blind.score) == false)
        {
            Debug.Log(stageButton.blind.score);

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

        // 현재 족보의 하위 족보 가져오기
        List<string> handTypes = PokerManager.Instance.GetContainedHandTypes();

        foreach (var joker in myJokerCard.Cards)
        {
            Debug.Log($"[조커 효과] 조커 이름: {joker.name}, 조건: {joker.currentData.baseData.require}");

            var context = new JokerEffectContext
            {
                StateManager = StateManager.Instance,
                MyJoker = joker,
                MyJokerCard = myJokerCard,
                HandTypes = handTypes,
                SelectedCards = selectedCards
            };
            var effect = joker.GetEffect(); // IJokerEffect 타입

            bool effectApplied = joker.ActivateEffect(context);


            if (effectApplied)
            {
                // 애니메이션 시퀀스를 별도로 가져와서 기다림
                if (effect is JokerCardEffect concreteEffect)
                {
                    var seq = concreteEffect.GetAnimationSequence();

                    if (seq != null)
                    {
                        yield return seq.WaitForCompletion(); // 애니메이션이 모두 끝날 때까지 대기
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


        int multiple = StateManager.Instance.multiplyChipSetting.GetMultiply();
        int plus = StateManager.Instance.multiplyChipSetting.GetChip();

        ScoreManager.Instance.AddScore(multiple * plus);

        ScoreManager.Instance.AnimateScore(lastScore, ScoreManager.Instance.TotalScore);
        
        Debug.Log($"이전 점수 : {lastScore} , 현재 점수 : {ScoreManager.Instance.TotalScore}");
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



    // 디버프 적용하기
    public int ApplyBossDebuff(int saveNumber)
    {
        // 현재 블라인드 기준
        int blindIndex = StageManager.Instance.GetCurrentBlindIndex();

        BlindRound currentDebuff = StageManager.Instance.GetBlindAtCurrentEnty(blindIndex);


        // null이고, 보스가 아니라면 원래 itemdata.id 반환
        if (currentDebuff == null || !currentDebuff.isBoss || currentDebuff.bossDebuff == null)
            return saveNumber;

        // 카드 디버프만 처리 -> 카드 디버프가 아니라면 반환
        if (currentDebuff.bossDebuff is not ICardDebuff cardDebuff)
            return saveNumber;

        //// 보스가 아니라면 null 값
        //IBossDebuff bossDebuff = currentDebuff.bossDebuff;  

        var selectedCards = pokerManager.cardData.SelectCards;

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var card = selectedCards[i];

            // 아직 처리되지 않은 카드 중에서 saveNumber와 동일한 id를 가진 카드
            if (!savenumberCheck[i] && card.itemdata.id == saveNumber)
            {
                Debug.Log("여기 실행되었어요");
                // 이 카드에 디버프 적용 여부 확인
                return cardDebuff.ApplyDebuff(card) ? 0 : saveNumber;
            }
        }

        // 못 찾았으면 그대로
        return saveNumber;
    }

    public void Calculate()
    {
        if (Num.Count > 0)
        {
            // 큐에서 빼면서 체크
            int saveNumber = Num.Dequeue();


            // 디버프 적용: 문양이 조건에 부합하면 점수 0으로 대체
            int finalScore = ApplyBossDebuff(saveNumber);

            // 값 더하기 (MVP)
            StateManager.Instance.multiplyChipSetting.AddPlus(finalScore);
            
            // 애니메이션 호출
            animationManager.PlayCardAnime(SaveNumber(saveNumber));

            // 애니메이션이 끝나기 전까지는 텍스트가 업데이트 되지 않기 때문에 텍스트 업데이트 이후에 로직 설정
            if (animationManager.moveTween != null && animationManager.moveTween.IsPlaying()) return;

        }
     
    }


    // 애니메이션을 호출하기 위해 사용 -> 디버프로 인해 saveNumber가 0이 되었을 경우 애니메이션이 실행되지 않음 & 0번부터 애니메이션이 들어감 -> 처음부터 들어가야함
    //
    // -> X 인덱스 기반으로 처리 (어차피 계산될 때 카드들은 오름차순으로 정렬됨)
    //
    // -> 안돼. 인덱스 기반으로 처리하면 saveNum에 들어가지 않은 애들도 애니메이션 나옴
    private GameObject game;

    private bool[] savenumberCheck = new bool[5];

    // 매개변수로 saveNum의 값 들어옴
    public GameObject SaveNumber(int saveNumber)
    {
        var selectedCards = pokerManager.cardData.SelectCards;

        // 현재 블라인드 기준
        int blindIndex = StageManager.Instance.GetCurrentBlindIndex();
        
        BlindRound currentDebuff = StageManager.Instance.GetBlindAtCurrentEnty(blindIndex);


        ICardDebuff cardDebuff = null;

        // 보스인지 아닌지 체크
        if (currentDebuff != null && currentDebuff.isBoss && currentDebuff.bossDebuff != null &&
            currentDebuff.bossDebuff is ICardDebuff debuff)
        {
            cardDebuff = debuff;
        }

        // |---

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var card = selectedCards[i];

            if (!savenumberCheck[i] && card.itemdata.id == saveNumber)
            {
                savenumberCheck[i] = true;
                game = card.gameObject;

                int finalScore = saveNumber;

                // 보스 디버프가 null이 아니라면 -> 현재 보스 블라인드!!
                if (currentDebuff != null && cardDebuff != null && cardDebuff.ApplyDebuff(card))
                {
                    finalScore = 0;
                    Debug.Log($"[디버프 적용] {card.itemdata.suit}{card.itemdata.id} → 점수: 0");
                }

                ShowRankText = card.GetComponent<ShowRankText>();
                ShowRankText.OnSettingRank(finalScore);

                ServiceLocator.Get<IAudioServicePitch>().PlaySFXPitch("Sound-CheckCard");
                return game;
            }
        }

        return null;
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
