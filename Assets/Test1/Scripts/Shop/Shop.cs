using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    // ��Ŀ ī�� Ÿ�� -> ���尡 �����Ҽ��� ���� ��Ŀ�� ��������
    [SerializeField] List<JokerCard> jokerCards;

    [SerializeField] List<JokerCard> shopjokerCards;

    // ��Ŀ�� ������ ��ġ 
    [SerializeField] GameObject jokerTransform;

    // ������ ���� ��Ŀ �� (���尡 �����ϸ� �ִ� 3������)
    [SerializeField] int maxCount;

    // |---------------------------------------------------------

    [SerializeField] MyJokerCard myJokerCards;

    public int currentRound;

    // |---------------------------------------------------------

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // �� ��Ŀ ī�� ������

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
        // ������ ��Ŀ ����
        //GenerateShopJokers();        
    }

    [SerializeField] private List<JokerCard> shopJokers; // ������ �ִ� ��Ŀ ī�� ������Ʈ 2��

    public void OpenShop()
    {
        for (int i = 0; i < shopJokers.Count; i++)
        {
            if (jokerManager.jokerBuffer.Count == 0)
            {
                shopJokers[i].gameObject.SetActive(false);
                continue;
            }

            // 1. ��Ŀ ���� ��������
            JokerTotalData selected = jokerManager.jokerBuffer[0];
            jokerManager.jokerBuffer.RemoveAt(0);

            // 2. ��Ŀ ������Ʈ�� ������ ����
            shopJokers[i].SetJokerData(selected);

            // 3. UI Ȱ��ȭ
            shopJokers[i].gameObject.SetActive(true);
        }
    }

    // | --------------------------------------------------

    public Button buyButton; // �ν����Ϳ� ����

    private JokerCard currentTarget;

    [SerializeField] GameObject jokerPacksTransform;

    public void ShowBuyButton(JokerCard target)
    {
        currentTarget = target;

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // ���ϴ� ������

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

        // �� ��Ŀ ������ ����
        GameObject newCard = Instantiate(myJokerPrefab, jokerTransform.transform);
        JokerCard cardScript = newCard.GetComponent<JokerCard>();
        cardScript.SetJokerData(data);

        // ���� ī�� ��Ȱ��ȭ
        currentTarget.DisableCard();

        // ��ư ����
        buyButton.gameObject.SetActive(false);
        currentTarget = null;
    }
}
