using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

// ���� ���� - ��Ŀ �߰�, Ÿ�� ī�� �߰�, �༺ ī�� �߰�, �ٿ�ó �߰�
public class Shop : MonoBehaviour
{
    // ��Ŀ ī�� Ÿ�� -> ���尡 �����Ҽ��� ���� ��Ŀ�� ��������
    [SerializeField] List<JokerCard> jokerCards;

    [SerializeField] List<JokerCard> shopjokerCards;

    // ��Ŀ�� ������ ��ġ 
    [SerializeField] GameObject jokerTransform;

    // |---------------------------------------------------------
    
    [SerializeField] MyJokerCard myJokerCards;

    public int currentRound;

    // |---------------------------------------------------------

    [SerializeField] JokerManager jokerManager;

    [SerializeField] private GameObject myJokerPrefab; // �� ��Ŀ ī�� ������

    // |---------------------------------------------------------

    [SerializeField] Money money;

    [SerializeField] ShopJokerPanel jokerPanel;

    private void Awake()
    {
        currentRound = Round.Instance.Enty;

        // |------------------------
        buyButton.gameObject.SetActive(false);
    }

    private void Start()
    {
        emptyPanel.gameObject.SetActive(false);
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

    [SerializeField] private JokerCard currentTarget;

    public JokerCard CurrentTarget => currentTarget;

    // Ŭ���� ī������ �Ǻ� 
    public void SetTarget(JokerCard card)
    {
        currentTarget = card;
    }

    public void ClearTarget()
    {
        currentTarget = null;
    }

    // | --------------------------------------------------

    [SerializeField] GameObject jokerPacksTransform;

    [SerializeField] GameObject emptyPanel;

    public void ShowBuyButton(JokerCard target)
    {
        currentTarget = target;

        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = buyButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(170, 50, 0); // ���ϴ� ������

        buyButton.gameObject.SetActive(true);
        emptyPanel.SetActive(true); // Ŭ�� ���ܱ� �ѱ�
    }

    public void OffBuyButton()
    {
        buyButton.gameObject.SetActive(false);
        emptyPanel.SetActive(false); // Ŭ�� ���ܱ� ����
        // ��ġ�� ������ ���ϱ�
    }
    public void OnEmptyClicked()
    {
        OffBuyButton();       
        currentTarget = null;  
    }

    // |------------------------------------

    [SerializeField] Button sellButton;

    public void ONSellButton(JokerCard target)
    {
        // ��ġ�� ��Ŀ ī�� ���� �̵�
        RectTransform buttonRect = sellButton.GetComponent<RectTransform>();
        buttonRect.position = target.transform.position + new Vector3(165, 50, 0); // ���ϴ� ������

        sellButton.gameObject.SetActive(true);
        fullScreenBlocker.SetActive(true); // Ŭ�� ���ܱ� �ѱ�
    }
    public void OffSellButton()
    {
        sellButton.gameObject.SetActive(false);
        fullScreenBlocker.SetActive(false); // Ŭ�� ���ܱ� ����
    }



    // �����ϱ�
    public void Buy()
    {
        if (currentTarget == null) return;

        // ��Ŀ�� 5�� �̻� & ���� ��Ŀ �ݾ� �̸� �̸� ���� �Ұ��� 
        if(myJokerCards.Cards.Count >= 5)
        {
            jokerPanel.OnOverJokerCount();
            return;
        }
        else if(money.TotalMoney < currentTarget.currentData.baseData.cost)
        {
            jokerPanel.OnNoBalance();
            return;
        }
        
        // ���� �ӴϿ��� ����
        money.MinusMoney(currentTarget.currentData.baseData.cost);

        // �Ӵ� UI������Ʈ
        money.MoneyUpdate();

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

        emptyPanel.gameObject.SetActive(false);
    }

    // �Ǹ��ϱ� 
    public void Sell()
    {
        if (currentTarget == null) return;

        Debug.Log("�Ǹŵǳ���?");
        // 1. ����Ʈ���� ����
        myJokerCards.RemoveJokerCard(currentTarget);

        // 2. ī�� ������Ʈ ��Ȱ��ȭ
        currentTarget.DisableCard();

        // 3. �� ȯ�� (��: ������ 50% ȸ��)
        int refundAmount = currentTarget.currentData.baseData.cost / 2;
        money.AddMoney(refundAmount);
        money.MoneyUpdate();

        // 4. ���� �ʱ�ȭ
        currentTarget = null;
        sellButton.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ShopPanel.OnShopOpened += OpenShop;
    }

    private void OnDisable()
    {
        ShopPanel.OnShopOpened -= OpenShop;
    }

    [SerializeField] private GameObject fullScreenBlocker;

    public void OnSellJokerPopups()
    {
        jokerPanel.OnSellJokerPopup();
        fullScreenBlocker.SetActive(false);
    }

    public void OnBlockerClicked()
    {
        OffSellButton();       // �Ǹ� ��ư ��Ȱ��ȭ
        currentTarget = null;  // ���� ī�� �ʱ�ȭ
    }


}
