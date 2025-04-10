using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    // 조커 카드 타입 -> 라운드가 증가할수록 높은 조커가 나오도록
    [SerializeField] List<JokerCard> jokerCards;

    // 조커가 생성될 위치 
    [SerializeField] GameObject jokerTransform;

    // 상점에 나올 조커 수 (라운드가 증가하면 최대 3개까지)
    [SerializeField] int maxCount;

    // |---------------------------------------------------------

    [SerializeField] MyJokerCard myJokerCards;

    public int currentRound;

    private void Awake()
    {
        maxCount = 2;

        currentRound = Round.Instance.Enty;

        // |------------------------
        buyButton.gameObject.SetActive(false);
        buyButton.onClick.AddListener(Buy);
    }

    private void Start()
    {
        GenerateShopJokers();
    }


    public void GenerateShopJokers()
    {
        // 현재 라운드에서 등장 가능한 조커만 필터링
        List<JokerCard> available = jokerCards.FindAll(card => card.unlockRound <= currentRound);

        if (available.Count == 0)
        {
            Debug.LogWarning("등장 가능한 조커 카드가 없습니다.");
            return;
        }

        // 리스트를 섞기
        for (int i = 0; i < available.Count; i++)
        {
            int randIndex = Random.Range(i, available.Count);
            var temp = available[i];
            available[i] = available[randIndex];
            available[randIndex] = temp;
        }

        // maxCount 만큼 중복 없이 뽑아서 생성
        for (int i = 0; i < maxCount && i < available.Count; i++)
        {
            Instantiate(available[i], jokerTransform.transform);
        }
    }


    // | --------------------------------------------------

    public Button buyButton; // 인스펙터에 연결
    private JokerCard currentTarget;


    public void ShowBuyButton(JokerCard target)
    {
        currentTarget = target;

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // 원하는 오프셋

        buyButton.gameObject.SetActive(true);
    }

    void Buy()
    {
        if (currentTarget != null)
        {
            Debug.Log($"구매한 조커: {currentTarget.dataSO.name}");
            // 구매 로직 실행
            myJokerCards.AddJokerCard(currentTarget);
        }
        buyButton.gameObject.SetActive(false);
    }
}
