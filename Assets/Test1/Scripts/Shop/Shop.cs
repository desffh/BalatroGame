using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


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

    // |---------------------------------------------------------

    [SerializeField] Money money;

    private void Awake()
    {
        maxCount = 2;

        currentRound = Round.Instance.Enty;

        // |------------------------
        buyButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        
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
        // 위치는 오프셋 정하기
    }

    // |------------------------------------

    [SerializeField] Button sellButton;

    public void ONSellButton(JokerCard target)
    {
        // 위치를 조커 카드 위로 이동
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(165, 50, 0); // 원하는 오프셋

        sellButton.gameObject.SetActive(true);
    }
    public void OffSellButton()
    {
        sellButton.gameObject.SetActive(false);
    }


    public void Buy()
    {
        if (currentTarget == null) return;

        // 조커가 5개 이상이라면 구매 불가능
        if(myJokerCards.Cards.Count >= 5)
        {
            Debug.Log("더 이상 구매할 수 없음");
            return;
        }

        // 데이터 복사
        JokerData data = currentTarget.GetData();
        Sprite sprite = currentTarget.GetSprite();

        // 내 조커 영역에 생성
        GameObject newCard = Instantiate(myJokerPrefab, jokerTransform.transform);
        JokerCard cardScript = newCard.GetComponent<JokerCard>();
        
        // 생성된 조커의 JokerCard 내부에 데이터 저장
        cardScript.SetJokerData(new JokerTotalData(data, sprite));

        // 내 조커 카드에 담기(정보만)
        myJokerCards.AddJokerCard(cardScript);

        // 상점 카드 비활성화
        currentTarget.DisableCard();

        // 버튼 숨김
        buyButton.gameObject.SetActive(false);
        currentTarget = null;
    }

    private void OnEnable()
    {
        ShopPanel.OnShopOpened += OpenShop;
    }

    private void OnDisable()
    {
        ShopPanel.OnShopOpened -= OpenShop;
    }
}
