using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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


    [SerializeField] TextMeshProUGUI TotalCards;
    [SerializeField] TextMeshProUGUI HandCards;

   private void Start()
   {
   
       // �ϴ� �ѹ� ȣ���ؼ� UI �� ����
       BufferUpdate();
       HandCardUpdate();
   }

    private void Update()
    {
        HandCardUpdate();
    }

    public void BufferUpdate()
    {
        TotalCards.text = KardManager.Instance.itemBuffer.Count.ToString()
            + " / "+ KardManager.Instance.itemBuffer.Capacity.ToString();
    }

    public void HandCardUpdate()
    {
        HandCards.text = (KardManager.Instance.myCards.Capacity - PokerManager.Instance.CardIDdata.Count).ToString() + " / "
            + KardManager.Instance.myCards.Capacity.ToString();
    }

}
