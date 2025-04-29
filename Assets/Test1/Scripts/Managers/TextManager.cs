using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextManager : Singleton<TextManager>
{
    // ���� UI Ŭ������ �迭�� ����
    [SerializeField] private NumTextUpdater[] numTextUpdaters;

    [SerializeField] private StringTextUpdater[] stringTextUpdaters;

    // �ؽ�Ʈ UI ������Ʈ ȣ��
    public void UpdateText(int index, int value = 0)
    {
        if (index >= 0 && index < numTextUpdaters.Length)
        {
            numTextUpdaters[index].UpdateText(value);  // �ε����� ���� �����Ͽ� �ؽ�Ʈ ����
        }
        else
        {
            Debug.LogWarning("Invalid UI index: " + index);
        }
    }

    // ��Ŀ �ؽ�Ʈ ����
    public void stringUpdateText(int index, string value = "")
    {
        if (index >= 0 && index < numTextUpdaters.Length)
        {
            stringTextUpdaters[index].UpdateText(value);  // �ε����� ���� �����Ͽ� �ؽ�Ʈ ����
        }
        else
        {
            Debug.LogWarning("Invalid UI index: " + index);
        }
    }



    // |-------------------------------------------------


    [SerializeField] AnimationManager animationmanager;

    private RectTransform plusTextPosition;
    private RectTransform MultiTextPosition;

    // |-------------------------------------------------

    [SerializeField] TextMeshProUGUI TotalCards;
    [SerializeField] TextMeshProUGUI HandCards;
    [SerializeField] TextMeshProUGUI TotalJokerCards;


    private void Start()
   {
       // �ϴ� �ѹ� ȣ���ؼ� UI �� ����
       HandCardUpdate();
       BufferUpdate();
   }


    private void Update()
    {
        HandCardUpdate();
    }


    // ���� ��, ī�� �Ѹ���, ���� �� �� ȣ��
    public void BufferUpdate()
    {
        int total = 52;
        int used = CardManager.Instance.totalSpawnedCount;
        int remaining = total - used;

        TotalCards.text = $"{remaining} / {total}";
    }

    public void HandCardUpdate()
    {
        HandCards.text = (CardManager.Instance.myCards.Capacity - PokerManager.Instance.cardData.SelectCards.Count).ToString() + " / "
            + CardManager.Instance.myCards.Capacity.ToString();
    }

    // ��Ŀ �ؽ�Ʈ ������Ʈ
    public void UpdateJokerCards(int count)
    {
        int total = 5;

        TotalJokerCards.text = count.ToString() + " / " + total; 
    }

}
