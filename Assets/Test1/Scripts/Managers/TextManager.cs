using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : Singleton<TextManager>
{
    // 여러 UI 클래스를 배열로 관리
    [SerializeField] private NumTextUpdater[] numTextUpdaters;

    [SerializeField] private StringTextUpdater[] stringTextUpdaters;

    // 텍스트 UI 업데이트 호출
    public void UpdateText(int index, int value = 0)
    {
        if (index >= 0 && index < numTextUpdaters.Length)
        {
            numTextUpdaters[index].UpdateText(value);  // 인덱스를 통해 접근하여 텍스트 갱신
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
            stringTextUpdaters[index].UpdateText(value);  // 인덱스를 통해 접근하여 텍스트 갱신
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
   
       // 일단 한번 호출해서 UI 싹 갱신
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
