using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    // 조커 카드 타입 -> 라운드가 증가할수록 높은 조커가 나오도록
    [SerializeField] List<JokerCard> jokerCards;

    [SerializeField] List<JokerCard> shopjokerCards;

    // 조커가 생성될 위치 
    [SerializeField] GameObject jokerTransform;

    // 상점에 나올 조커 수 (라운드가 증가하면 최대 3개까지)
    [SerializeField] int maxCount;

    // |---------------------------------------------------------

    [SerializeField] MyJokerCard myJokerCards;

    public int currentRound;

    // |---------------------------------------------------------

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // 내 조커 카드 프리팹

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
    }

    private void OnEnable()
    {
        // 상점에 조커 생성
        //GenerateShopJokers();        
    }

    [SerializeField] private List<JokerCard> shopJokers; // 상점에 있는 조커 카드 오브젝트 2개

    public void OpenShop()
    {
        for (int i = 0; i < shopJokers.Count; i++)
        {
            if (jokerManager.jokerBuffer.Count == 0)
            {
                shopJokers[i].gameObject.SetActive(false);
                continue;
            }

            // 1. 조커 정보 가져오기
            JokerTotalData selected = jokerManager.jokerBuffer[0];
            jokerManager.jokerBuffer.RemoveAt(0);

            // 2. 조커 오브젝트에 데이터 적용
            shopJokers[i].SetJokerData(selected);

            // 3. UI 활성화
            shopJokers[i].gameObject.SetActive(true);
        }
    }

    // | --------------------------------------------------

    public Button buyButton; // 인스펙터에 연결

    private JokerCard currentTarget;

    [SerializeField] GameObject jokerPacksTransform;

    public void ShowBuyButton(JokerCard target)
    {
        currentTarget = target;

        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // 원하는 오프셋

        buyButton.gameObject.SetActive(true);
    }

    public void OffBuyButton()
    {
        buyButton.gameObject.SetActive(false);
    }

    public void Buy()
    {
        if (currentTarget == null) return;

        JokerTotalData data = currentTarget.GetCurrentData();

        // 내 조커 영역에 생성
        GameObject newCard = Instantiate(myJokerPrefab, jokerTransform.transform);
        JokerCard cardScript = newCard.GetComponent<JokerCard>();
        cardScript.SetJokerData(data);

        // 상점 카드 비활성화
        currentTarget.DisableCard();

        // 버튼 숨김
        buyButton.gameObject.SetActive(false);
        currentTarget = null;
    }
}
