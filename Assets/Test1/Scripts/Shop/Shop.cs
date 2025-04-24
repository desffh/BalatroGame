using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


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
        // ��ġ�� ������ ���ϱ�
    }

    // |------------------------------------

    [SerializeField] Button sellButton;

    public void ONSellButton(JokerCard target)
    {
        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(165, 50, 0); // ���ϴ� ������

        sellButton.gameObject.SetActive(true);
    }
    public void OffSellButton()
    {
        sellButton.gameObject.SetActive(false);
    }


    public void Buy()
    {
        if (currentTarget == null) return;

        // ��Ŀ�� 5�� �̻��̶�� ���� �Ұ���
        if(myJokerCards.Cards.Count >= 5)
        {
            Debug.Log("�� �̻� ������ �� ����");
            return;
        }

        // ������ ����
        JokerData data = currentTarget.GetData();
        Sprite sprite = currentTarget.GetSprite();

        // �� ��Ŀ ������ ����
        GameObject newCard = Instantiate(myJokerPrefab, jokerTransform.transform);
        JokerCard cardScript = newCard.GetComponent<JokerCard>();
        
        // ������ ��Ŀ�� JokerCard ���ο� ������ ����
        cardScript.SetJokerData(new JokerTotalData(data, sprite));

        // �� ��Ŀ ī�忡 ���(������)
        myJokerCards.AddJokerCard(cardScript);

        // ���� ī�� ��Ȱ��ȭ
        currentTarget.DisableCard();

        // ��ư ����
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
