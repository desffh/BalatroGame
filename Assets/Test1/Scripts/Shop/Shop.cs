using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    // ��Ŀ ī�� Ÿ�� -> ���尡 �����Ҽ��� ���� ��Ŀ�� ��������
    [SerializeField] List<JokerCard> jokerCards;

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
        GenerateShopJokers();
    }


    public void GenerateShopJokers()
    {
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
        }
    }


    // | --------------------------------------------------

    public Button buyButton; // �ν����Ϳ� ����
    private JokerCard currentTarget;


    public void ShowBuyButton(JokerCard target)
    {
        currentTarget = target;

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // ���ϴ� ������

        buyButton.gameObject.SetActive(true);
    }

    void Buy()
    {
        if (currentTarget != null)
        {
            Debug.Log($"������ ��Ŀ: {currentTarget.dataSO.name}");
            // ���� ���� ����
            myJokerCards.AddJokerCard(currentTarget);
        }
        buyButton.gameObject.SetActive(false);
    }
}
