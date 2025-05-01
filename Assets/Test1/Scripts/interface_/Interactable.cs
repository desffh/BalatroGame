using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Interactable : MonoBehaviour
{
    [SerializeField] Button rankButton;


    private void Awake()
    {
        rankButton = GetComponent<Button>();
    }

    private void Update()
    {
        // 이것도 나중에 카드가 클릭 되었을때만 판별하도록 하면 Update()에서 사용안해도됨
        if (PokerManager.Instance.cardData.SelectCards.Count > 0)
        {
            OffButton();
        }
        else
        {
            OnButton();
        }
    }

    public void OffButton()
    {
        rankButton.interactable = false;
    }

    public void OnButton()
    {
        //Debug.Log("랭크 버튼 활성화");
        rankButton.interactable = true;
    }
}
