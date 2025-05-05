using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.Pool;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.Serialization.Formatters;



public class CardManager : Singleton<CardManager>
{
    // ����
    [SerializeField] ItemDataReader ItemDataReader;
    [SerializeField] Transform deleteSpawn;
    [SerializeField] public Card card;

    // ItemData Ÿ���� ���� List (�� 52��)
    [SerializeField] public List<ItemData> itemBuffer;

    [SerializeField] private List<ViewCard> viewerCards; // �����Ϳ� �̸� ��ġ�� 52��

    // Card Ÿ���� ���� ����Ʈ (�� ī�� : �ִ� 8��)
    [SerializeField] public List<Card> myCards;

    // |----------------------------------------

    // ī�� ������ġ
    [SerializeField] public Transform cardSpawnPoint;

    // ī�� ���� ����, �� ��ġ
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;

    // �����Ϳ��� ī�� ������ ���� (Instantiate)
    [SerializeField] GameObject cardPrefabs;

    // �θ� ���� ������Ʈ �Ҵ� (��ġ�� ��ġ)
    [SerializeField] GameObject ParentCardPrefab;

    // |------------------------------

    // ������Ʈ Ǯ�� ����
    //
    // CardManager.pools : ī�� Ǯ ��ü�� �����ϴ� �߾� ������
    //                     
    //                     ī�尡 �ʿ��� �� pools.Get() ���� ������,
    //                     ī�带 �ݳ��� �� pools.Release(card)�� ����������
    // |------------------------------

    // ����� ī��� (Ǯ�� ��ȯ�뵵)
    [SerializeField] public List<Card> usedCards;

    private IObjectPool<Card> pools;

    // |------------------------------

    protected override void Awake()
    {
        base.Awake();

        // ������Ʈ Ǯ �ʱ�ȭ -> ī�� ����/ȹ��/�ݳ�/���� �� ���� �ݹ� �Լ� ���
        pools = new ObjectPool<Card>(CreateCard, OnGetCard, OnReleaseCard, OnDestroyCard, maxSize: 52);
    }

    private void Start()
    {
        SetupViewerCards();
        ReactivateAllViewerCards(); // ó���� �� ī�� �� ���̵���
    }

    // |------------------------------

    // ����� ī����� usedCards�� �߰� (���߿� Ǯ�� ��ȯ �� ī���)
    public void OnCardUsed(Card card)
    {
        if (!usedCards.Contains(card))
        {
            usedCards.Add(card);
        }
        else
        {
            Debug.LogWarning($"[�ߺ����]: {card.name}");
        }

        // ���� ī����� �� ī�� ��Ȱ��ȭ ó��
        SyncViewerCards();
    }

    // (Ǯ�� ����� ��) ī�� ���� �� ȣ�� �� �Լ�
    private Card CreateCard()
    {
        Card cardObject = Instantiate(cardPrefabs, cardSpawnPoint.position, Utils.QI).GetComponent<Card>(); // ���� ������Ʈ Ÿ��

        cardObject.SetManagedPool(pools); // ī�� ���� �� Ǯ�� �ֱ�

        return cardObject;
    }

    // Ǯ���� ������Ʈ�� ���� �� ���Ǵ� �Լ�
    private void OnGetCard(Card card)
    {
        if (card.isInUse)
        {
            Debug.LogWarning($"[Ǯ�� ����] �̹� ������� ī��: {card.name}");
        }
        card.isInUse = true;

        //card.OffCollider();

        card.gameObject.SetActive(true);
    }

    // Ǯ�� ������Ʈ�� �ݳ��� �� ���Ǵ� �Լ�
    private void OnReleaseCard(Card card)
    {
        card.isInUse = false;

        //card.OffCollider();

        card.gameObject.SetActive(false);
    }

    // Ǯ���� ������Ʈ �ı� �� ȣ�� �� �Լ�
    private void OnDestroyCard(Card card)
    {
        Destroy(card.gameObject);
    }

    // |------------------------------

    // ����� ī����� ��ġ&ȸ�� ���� (cardSpawnPoint�� �̵�) 
    public void TransCard()
    {
        for (int i = 0; i < usedCards.Count; i++)
        {
            usedCards[i].GetComponent<Transform>().gameObject.transform.position = cardSpawnPoint.transform.position;
            usedCards[i].GetComponent<Transform>().gameObject.transform.rotation = cardSpawnPoint.transform.rotation;

        }
    }

    // ī�尡 Ŭ���Ǿ����� Ȯ���ϴ� bool ���� �ʱ�ȭ
    public void CheckCards()
    {
        for (int i = 0; i < usedCards.Count; i++)
        {
            usedCards[i].GetComponent<Card>().checkCard = false;
        }
    }

    // ���� �� ���ο� ī��� ��ġ�Ǹ� �˾��� �ؽ�Ʈ�� �ʱ�ȭ 
    public void CheckTexts()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].GetComponent<Card>().PopupText();
        }
    }


    // ���ۿ� ī�� �ֱ� (52)
    void SetupItemBuffer()
    {
        // ũ�� �����Ҵ�
        if (itemBuffer == null)
        {
            itemBuffer = new List<ItemData>(52);
        }
        else
        {
            itemBuffer.Clear();
        }

        // �� Capacity ũ�Ⱑ 64��?

        for (int i = 0; i < 52; i++)
        {
            // ������ �������� ��Ʈ ������ ���� ī����� itemBuffer�� ����
            ItemData item = ItemDataReader.DataList[i];

            itemBuffer.Add(item);
        }
        // ������ ���� ���� ī�� ����
        for (int i = 0; i < itemBuffer.Count; i++)
        {
            int rand = Random.Range(i, itemBuffer.Count);
            ItemData temp = itemBuffer[i];
            itemBuffer[i] = itemBuffer[rand];
            itemBuffer[rand] = temp;
        }
    }

    // ���ۿ��� ī�� �̱� (Ǯ���� ī�� ������)
    public ItemData PopItem()
    {
        ItemData item = itemBuffer[0];
        itemBuffer.RemoveAt(0); // ����Ʈ �޼��� (0��° ��� ����)
        return item;
    }

    public int totalSpawnedCount = 0; // ī�� �Ѹ� �� ����

    // 8���� ��ġ�� ī�� �߰� 
    void AddCard()
    {
        if (myCards.Count < 8)
        {
            Card cardObject = pools.Get();

            // �θ��� �Ʒ��� ���� (���̶�Űâ ��������)
            cardObject.transform.SetParent(ParentCardPrefab.transform);

            // ���� ������ī�� ������Ʈ
            Card card = cardObject.GetComponent<Card>(); // ������ ī���� ��ũ��Ʈ �������� (Card)

            card.Setup(PopItem()); // ���� ī�忡 ItemData ���� ���� & ��������Ʈ ����

            //card.OffCollider();

            // myCards ����Ʈ�� ����
            if (!myCards.Contains(card))
            {
                myCards.Add(card);

                totalSpawnedCount++;
            }

            ServiceLocator.Get<IAudioService>().PlaySFX("Sound-SpawnCard");
        }
        CheckTexts();

        TextManager.Instance.BufferUpdate(); // �׻� �ؽ�Ʈ ����

        // ī�� ����
        SetOriginOrder();
    }

    // ����Ʈ ��ü ���� (���� �߰��� ī�尡 ���� ���ʿ� ����)
    public void SetOriginOrder()
    {
        int count = myCards.Count;

        for (int i = 0; i < count; i++)
        {
            var targetCard = myCards[i];

            // ? -> targerCard�� null�� �ƴϸ� ������Ʈ ��������
            targetCard?.GetComponent<Order>().SetOriginOrder(i);
        }
    }


    // ī�� ��ġ ����
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
                continue;
            }

            targetCard.originPRS = originCardPRSs[i];

            targetCard.MoveTransform(targetCard.originPRS, true, 1f, () =>
            {
                completeCount++;
                // ���� ��ġ ����(�� ���� �����ϰ� �ʹٸ� bool�� üũ)
                targetCard.SaveInitialTransform();

                if (completeCount >= cardCount)
                {
                    onAllComplete?.Invoke();
                }
            });
        }

    }

    // ī�� ���� ����
    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount); // objCount ��ŭ �뷮 �̸� �Ҵ�

        switch (objCount)
        {
            // ������ : 1,2,3�� �϶� (ȸ���� ���� ����)
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3: objLerps = new float[] { 0.1f, 0.5f, 0.9f }; break;

            // ī�尡 4�� �̻��϶� ���� ȸ������ ��
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        // ���� ������
        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Utils.QI;

            // ī�尡 4�� �̻��̶�� ȸ���� ��
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

    // |------------------------------

    // ī�带 myCards �� ��ġ (ī�� �߰�)
    public void AddCardSpawn(System.Action onComplete = null)
    {
        StartCoroutine(SpawnCardsSequentially(onComplete));

        // �̺κ��� �ǹ̰� ���� �� ���� -> myCards.Count�� 8�� �Ǿ�� ���� �ִϸ��̼��� ������ ���� ����
        if (myCards.Count == 8)
        {
            for (int i = 0; i < myCards.Count; i++)
            {
                Debug.Log("�� ī�� Ŭ�� �ʱ�ȭ");
                myCards[i].GetComponent<Card>().checkCard = false;
            }
        }
    }

    private IEnumerator SpawnCardsSequentially(System.Action onComplete = null)
    {
        for (int i = myCards.Count; i < 8; i++)
        {
            AddCard();

            // ���� �� �ִϸ��̼� �Ϸ���� �� �ݶ��̴� �ٽ� �ѱ�
            CardAlignment(() =>
            {
                TurnOnAllCardColliders();
            });

                yield return new WaitForSeconds(0.15f);
        }
        //Debug.Log("��� ī�� ��ġ �Ϸ� �� onComplete ����");

        onComplete?.Invoke(); // �ݶ��̴� �ѱ�
    }

    // ��ũ ���� ��ư Ŭ�� ��
    public void Allignment()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        myCards = myCards.OrderBy(card => card.itemdata.id).ToList();
        SetOriginOrder();
        CardAlignment(() =>
        {
            TurnOnAllCardColliders();
        });
    }

    // |------------------------------

    // ���� �������� ����
    public void SetupNextStage()
    {
        // OnUsedCard �� �߰�
        deleteCards();

        ReturnAllCardsToPool(); // Ǯ�� ���� ī�� ��ȯ

        StartSettings();
    }

    public void StartSettings()
    {
        StartCoroutine(Settings());
    }

    // ��ġ�� myCards ī�� ��� �ݶ��̴� Ȱ��ȭ
    public void TurnOnAllCardColliders()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            myCards[i].OnCollider();
        }
    }

    // ��ġ�� myCards ī�� ��� �ݶ��̴� ��Ȱ��ȭ
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

        // ����Ʈ �ʱ�ȭ
        PokerManager.Instance.cardData.ClearSelectCard();
        PokerManager.Instance.saveNum.Clear();

        // ī�� ä���
        SetupItemBuffer();

        // ī�� �Ѹ���, ���� ������, �ݶ��̴� �ѱ�
        AddCardSpawn(() => {
            //TurnOnAllCardColliders();
        });

        ButtonManager.Instance.ButtonInactive();

        // ��ũ ���� ��ư Ȱ��ȭ
        HoldManager.Instance.interactable.OnButton();

        // �ڵ� & ������ ī��Ʈ �ʱ�ȭ 
        HandDelete.Instance.ResetCounts();

        // UI ��� ������Ʈ
        HoldManager.Instance.UIupdate();
        HoldManager.Instance.TotalScoreupdate();

        HoldManager.Instance.CheckReset();

        HoldManager.Instance.RefillActionQueue();


        // �� ī�� �ʱ�ȭ
        ReactivateAllViewerCards();
        SetupViewerCards();           // �� ī�� ���� ����
    }


    // ��ġ�� ī�� & ������� ī�� �ݶ��̴� ��Ȱ��ȭ
    public void ColliderQuit()
    {
        TurnOffAllCardColliders();
        PokerManager.Instance.QuitCollider2();
    }


    // ����� ī��� Ǯ�� ��ȯ�ؾ���
    public void deleteCards()
    {
        for (int i = 0; i < myCards.Count; i++)
        {
            OnCardUsed(myCards[i]);
        }
    }

    // Ǯ�� ��ȯ�ϱ� (��� �ʱ�ȭ �� Ǯ�� ��ȯ)
    public void ReturnAllCardsToPool()
    {
        totalSpawnedCount = 0; // ī�� �Ѹ� �� ���� �ʱ�ȭ

        TransCard();

        CheckCards();

        // �ߺ� ������ ���� ��ȯ
        var distinctUsedCards = usedCards.Distinct().ToList();

        foreach (var card in distinctUsedCards)
        {
            pools.Release(card);
        }

        usedCards.Clear();
        myCards.Clear();
    }

    // |-------------------------

    //�� ī�� �Լ�
    public void SyncViewerCards()
    {
        // ���� ��� ���̰� ����
        foreach (ViewCard viewer in viewerCards)
        {
            viewer.GetComponent<ViewCard>().Show();
        }
    
        // ���� ī��� inherenceID�� ���� ��� ī�常 ����
        foreach (Card usedCard in usedCards)
        {
            int usedID = usedCard.itemdata.inherenceID;
    
            ViewCard matchedViewer = viewerCards.FirstOrDefault(v => v.cardID == usedID);
            if (matchedViewer != null)
            {
                //Debug.Log("ī�� �����");
                matchedViewer.Hide();
            }
        }
    }

    // �� ī���� �̹��� ��� ���̰� �ʱ�ȭ (�������� ���� �����Ҷ����� ȣ��)
    public void ReactivateAllViewerCards()
    {
        foreach (ViewCard viewer in viewerCards)
        {
            viewer.GetComponent<ViewCard>().Show();
        }
    }


    // �� ī�� ��������Ʈ ����
    public void SetupViewerCards()
    {
        for (int i = 0; i < viewerCards.Count; i++)
        {
            ViewCard viewerCard = viewerCards[i];
         
            viewerCard.GetComponent<ViewCard>().Setup(ItemDataReader.DataList[i]);
        }
    }

}
