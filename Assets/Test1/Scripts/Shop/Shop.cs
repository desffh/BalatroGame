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

    public void GenerateShopJokers()
    {
        // ����
        shopjokerCards.Clear();

        // ���� ���忡�� ���� ������ ��Ŀ�� ���͸�
        List<JokerCard> available = jokerCards.FindAll(card => card.unlockRound <= currentRound);

        if (available.Count == 0)
        {
            Debug.LogWarning("���� ������ ��Ŀ ī�尡 �����ϴ�.");
            return;
        }

        // ����Ʈ�� ����
        for (int i = 0; i < available.Count; i++)
        {
            int randIndex = Random.Range(i, available.Count);
            var temp = available[i];
            available[i] = available[randIndex];
            available[randIndex] = temp;
        }

        // maxCount ��ŭ �ߺ� ���� �̾Ƽ� ����
        for (int i = 0; i < maxCount && i < available.Count; i++)
        {
            Instantiate(available[i], jokerTransform.transform);

            shopjokerCards.Add(available[i]);
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

    void Buy()
    {
        if (currentTarget != null)
        {
            Debug.Log($"������ ��Ŀ: {currentTarget.dataSO.name}");
            // ���� ���� ����
            myJokerCards.AddJokerCard(currentTarget);

            currentTarget.transform.SetParent(jokerPacksTransform.transform, false);
        }
        buyButton.gameObject.SetActive(false);


    }
}
