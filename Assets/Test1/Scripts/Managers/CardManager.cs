using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.Pool;
using TMPro;
using Unity.VisualScripting;



public class CardManager : Singleton<CardManager>
{
    // 참조
    [SerializeField] ItemDataReader ItemDataReader;
    [SerializeField] Transform deleteSpawn;
    [SerializeField] public Card card;

    // ItemData 타입을 담을 List (총 52장)
    [SerializeField] public List<ItemData> itemBuffer;

    [SerializeField] private List<ViewCard> viewerCards; // 에디터에 미리 배치된 52장

    // Card 타입을 담을 리스트 (내 카드 : 최대 8장)
    [SerializeField] public List<Card> myCards;

    // 사용한 카드들 (풀링 반환용도)
    [SerializeField] public List<Card> usedCards;


    // |----------------------------------------

    // 카드 생성위치
    [SerializeField] public Transform cardSpawnPoint;

    // 카드 정렬 시작, 끝 위치
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;

    // 에디터에서 카드 프리팹 연결 (Instantiate)
    [SerializeField] GameObject cardPrefabs;

    // 부모 게임 오브젝트 할당 (배치될 위치)
    [SerializeField] GameObject ParentCardPrefab;

    // |------------------------------

    private IObjectPool<Card> pools;

    protected override void Awake()
    {
        base.Awake();

        pools = new ObjectPool<Card>(CreateCard, OnGetCard, OnReleaseCard, OnDestroyCard, maxSize: 52);
    }

    private void Start()
    {
        SetupViewerCards();
        ReactivateAllViewerCards(); // 처음엔 다 보이게
    }

    // 사용한 카드들
    public void OnCardUsed(Card card)
    {
        if (!usedCards.Contains(card))
        {
            usedCards.Add(card);
        }
        else
        {
            Debug.LogWarning($"[중복사용] 이미 usedCards에 있는 카드가 또 들어가려 함: {card.name}");
        }

        SyncViewerCards(); // 여기서 뷰 카드 비활성화 처리
    }

    // 사용한 카드들의 위치&회전 변경 (cardSpawnPoint로 이동) 
    public void TransCard()
    {
        for (int i = 0; i < usedCards.Count; i++)
        {
            usedCards[i].GetComponent<Transform>().gameObject.transform.position = cardSpawnPoint.transform.position;
            usedCards[i].GetComponent<Transform>().gameObject.transform.rotation = cardSpawnPoint.transform.rotation;

        }
    }

    // 카드가 클릭되었는지 확인하는 bool 변수 초기화
    public void CheckCards()
    {
        for (int i = 0; i < usedCards.Count; i++)
        {
            usedCards[i].GetComponent<Card>().checkCard = false;
        }
    }

    // 셔플 후 새로운 카드로 배치되면 팝업의 텍스트도 초기화 
    public void CheckTexts()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].GetComponent<Card>().PopupText();
        }
    }

    // 카드가 생성될 때 호출 될 함수
    private Card CreateCard()
    {
        Card cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI).GetComponent<Card>(); // 게임 오브젝트 타입
        cardObject.SetManagedPool(pools);
        return cardObject;
    }

    // 풀에서 오브젝트를 빌릴 때 사용되는 함수
    private void OnGetCard(Card card)
    {
        if (card.isInUse)
        {
            Debug.LogWarning($"[풀링 오류] 이미 사용 중인 카드를 다시 꺼냄: {card.name}");
        }
        card.isInUse = true;
        card.OffCollider();
        card.gameObject.SetActive(true);
    }


    // 풀에 오브젝트를 반납할 때 사용되는 함수
    private void OnReleaseCard(Card card)
    {
        card.isInUse = false;
        card.OffCollider();
        card.gameObject.SetActive(false);
    }


    // 풀에서 오브젝트 파괴 시 호출 될 함수
    private void OnDestroyCard(Card card)
    {
        Destroy(card.gameObject);
    }





    // 버퍼에 카드 넣기
    void SetupItemBuffer()
    {
        // 크기 동적할당
        if (itemBuffer == null)
        {
            itemBuffer = new List<ItemData>(52);
        }
        else
        {
            itemBuffer.Clear();
        }

        // 왜 Capacity 크기가 64지?

        for (int i = 0; i < 52; i++)
        {
            // 각각의 스프레드 시트 정보를 담은 카드들을 itemBuffer에 저장
            ItemData item = ItemDataReader.DataList[i];

            itemBuffer.Add(item);
        }
        // 아이템 버퍼 안의 카드 섞기
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            ItemData temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    // 버퍼에서 카드 뽑기
    public ItemData PopItem()
    {
        ItemData item = itemBuffer[0];
        itemBuffer.RemoveAt(0); // 리스트 메서드 (0번째 요소 제거)
        return item;
    }

    public int totalSpawnedCount = 0; // 카드 뿌린 총 개수

    // 8장의 배치될 카드 추가 
    void AddCard()
    {
        if (myCards.Count < 8)
        {
            //var cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI); // 게임 오브젝트 타입

            var cardObject = pools.Get();

            // 부모의 아래에 생성 (하이라키창 계층구조)
            cardObject.transform.SetParent(ParentCardPrefab.transform);

            // 동적 생성된카드 오브젝트
            var card = cardObject.GetComponent<Card>(); // 생성된 카드의 스크립트 가져오기 (Card)
            card.Setup(PopItem()); // 뽑은 카드에 ItemData 정보 저장 & 스프라이트 셋팅

            card.OffCollider();

            // 동적 생성된 카드 오브젝트는 Card 스크립트를 가지고 있어서 Card타입 리스트에 담을 수 있다
            if (!myCards.Contains(card))
            {
                myCards.Add(card);
                totalSpawnedCount++;
            }


            SoundManager.Instance.PlayCardSpawn();
        }
        CheckTexts();

        TextManager.Instance.BufferUpdate(); // 항상 텍스트 갱신

        // 카드 정렬
        SetOriginOrder();
        //CardAlignment();

    }

    // 리스트 전체 정렬 (먼저 추가한 카드가 제일 뒷쪽에 보임)
    public void SetOriginOrder()
    {
        int count = myCards.Count;

        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];

            // ? -> targerCard가 null이 아니면 컴포넌트 가져오기
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }
    // 카드 정렬
    public void CardAlignment(System.Action onAllComplete = null)
    {
        List<PRS> originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 0.7f);

        int completeCount = 0;
        int cardCount = myCards.Count;

        for (int i = 0; i < cardCount; i++)
        {
            var targetCard = myCards[i];

            if (targetCard == null)
            {
                Debug.LogError($"[CardAlignment] targetCard가 null입니다! index: {i}");
                continue;
            }

            targetCard.originPRS = originCardPRSs[i];

            Debug.Log($"[CardAlignment] 카드 배치 시작: {targetCard.name}");

            targetCard.MoveTransform(targetCard.originPRS, true, 0.6f, () => {
                completeCount++;
                Debug.Log($"[애니메이션 완료] 카드: {targetCard.name}, 현재 완료 수: {completeCount}");

                if (completeCount >= cardCount)
                {
                    Debug.Log("[CardAlignment] 모든 카드 애니메이션 완료");
                    onAllComplete?.Invoke();
                }
            });
        }

    }




    // 카드 원형 정렬
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount); // objCount 만큼 용량 미리 할당

        switch (objCount)
        {
            // 고정값 : 1,2,3개 일때 (회전이 없기 때문)
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;

            // 카드가 4개 이상일때 부터 회전값이 들어감
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        // 원의 방정식
        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;

            // 카드가 4개 이상이라면 회전값 들어감
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }
        return results;
    }

    public void AddCardSpawn(System.Action onComplete = null)
    {
        StartCoroutine(SpawnCardsSequentially(onComplete));
    }

    private IEnumerator SpawnCardsSequentially(System.Action onComplete = null)
    {
        bool lastMoveDone = false;

        int cardsNeeded = 8 - myCards.Count; // 현재 보유 수를 기준으로 부족한 만큼만 생성

        for (int i = 0; i < cardsNeeded; i++)
        {
            AddCard();

            if (i == cardsNeeded - 1)
            {
                // 마지막 카드 정렬 완료 시점에 콜백 설정
                CardAlignment(() => {
                    Debug.Log("[SpawnCardsSequentially] 마지막 카드 애니메이션 완료");
                    lastMoveDone = true;
                });
            }
            else
            {
                CardAlignment();
            }

            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitUntil(() => lastMoveDone);

        Debug.Log("[SpawnCardsSequentially] 모든 카드 배치 완료 → onComplete 실행");
        onComplete?.Invoke(); // 콜라이더 켜기
    }


    public void Allignment()
    {
        myCards = myCards.OrderBy(card => card.itemdata.id).ToList();
        SetOriginOrder();
        CardAlignment();
    }


    public void SetupNextStage()
    {
        deleteCards();

        ReturnAllCardsToPool(); // 기존 카드 반환

        StartSettings();
    }

    public void StartSettings()
    {
        StartCoroutine(Settings());
    }

    public void TurnOnAllCardColliders()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].OnCollider();
            Debug.Log($"[콜라이더 ON] 카드: {myCards[i].name}");
        }
    }

    public void TurnOffAllCardColliders()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].OffCollider();
        }
    }
    // |-------------------------

    public IEnumerator Settings()
    {
        yield return CoroutineCache.WaitForSeconds(1.0f);

        ScoreManager.Instance.ResetTotalScore();

        // 리스트 초기화
        PokerManager.Instance.cardData.ClearSelectCard();
        PokerManager.Instance.saveNum.Clear();

        // 카드 채우기
        SetupItemBuffer();

        // 카드 뿌리고, 정렬 끝나고, 콜라이더 켜기
        AddCardSpawn(() => {
            TurnOnAllCardColliders();
        });

        ButtonManager.Instance.ButtonInactive();

        // 랭크 정렬 버튼 활성화
        HoldManager.Instance.interactable.OnButton();

        // 핸드 & 버리기 카운트 초기화 
        HandDelete.Instance.ResetCounts();

        // UI 모두 업데이트
        HoldManager.Instance.UIupdate();
        HoldManager.Instance.TotalScoreupdate();

        HoldManager.Instance.CheckReset();

        HoldManager.Instance.RefillActionQueue();


        // 뷰 카드 초기화
        ReactivateAllViewerCards();
        SetupViewerCards();           // 뷰 카드 정보 설정
    }


    // 배치된 카드 & 계산중인 카드 콜라이더 비활성화
    public void ColliderQuit()
    {
        TurnOffAllCardColliders();
        PokerManager.Instance.QuitCollider2();
    }


    public void deleteCards()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            OnCardUsed(myCards[i]);
        }
    }

    // 풀에 반환하기 (모두 초기화 후 풀에 반환)
    public void ReturnAllCardsToPool()
    {
        totalSpawnedCount = 0; // 카드 뿌린 총 개수 초기화

        TransCard();

        CheckCards();

        // 중복 제거한 다음 반환
        var distinctUsedCards = usedCards.Distinct().ToList();

        foreach (var card in distinctUsedCards)
        {
            pools.Release(card);
        }

        usedCards.Clear();
        myCards.Clear();
    }

     //뷰 카드 함수
    public void SyncViewerCards()
    {
        // 먼저 모두 보이게 설정
        foreach (ViewCard viewer in viewerCards)
        {
            viewer.GetComponent<ViewCard>().Show();
        }
    
        // 사용된 카드와 ID가 같은 뷰어 카드만 숨김
        foreach (Card usedCard in usedCards)
        {
            int usedID = usedCard.itemdata.inherenceID;
    
            ViewCard matchedViewer = viewerCards.FirstOrDefault(v => v.cardID == usedID);
            if (matchedViewer != null)
            {
                //Debug.Log("카드 숨기기");
                matchedViewer.Hide();
            }
        }
    }


    public void ReactivateAllViewerCards()
    {
        foreach (ViewCard viewer in viewerCards)
        {
            viewer.GetComponent<ViewCard>().Show();
        }
    }


    public void SetupViewerCards()
    {
        //Debug.Log("[CardManager] SetupViewerCards() 호출됨");

        for (int i = 0; i < viewerCards.Count; i++)
        {

            ViewCard viewerCard = viewerCards[i];
            viewerCard.GetComponent<ViewCard>().Setup(ItemDataReader.DataList[i]);
        }
    }

}
