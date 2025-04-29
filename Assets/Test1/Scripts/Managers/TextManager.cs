using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    // 포커 텍스트 갱신
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

    // |-------------------------------------------------

    [SerializeField] TextMeshProUGUI TotalCards;
    [SerializeField] TextMeshProUGUI HandCards;
    [SerializeField] TextMeshProUGUI TotalJokerCards;


    private void Start()
   {
       // 일단 한번 호출해서 UI 싹 갱신
       HandCardUpdate();
       BufferUpdate();
   }


    private void Update()
    {
        HandCardUpdate();
    }


    // 시작 시, 카드 뿌릴때, 셋팅 할 때 호출
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

    // 조커 텍스트 업데이트
    public void UpdateJokerCards(int count)
    {
        int total = 5;

        TotalJokerCards.text = count.ToString() + " / " + total; 
    }

}
