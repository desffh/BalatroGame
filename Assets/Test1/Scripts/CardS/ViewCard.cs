using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// View�� ī�� ��ũ��Ʈ 

public class ViewCard : MonoBehaviour
{
    [SerializeField] private Image cardImage; // ī�� �̹��� (UI)

    [SerializeField] public int cardID { get; private set; }

    [SerializeField] public string suit;

    [SerializeField] public int rank;

    private CanvasGroup canvasGroup;

    [SerializeField] ViewCardText viewCardText;

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        popupTextSetting();
    }

    // |-----------------------------------------------------

    // �� ī�� �̹��� & ������ ����
    public void Setup(ItemData data)
    {
        cardID = data.inherenceID;

        suit = data.name;

        rank = data.id;

        // ī�� �̸��� �ش��ϴ� ��������Ʈ�� CardSprites���� ��������
        string spriteName = data.front;
        Sprite selectedSprite = CardSprites.Instance.Get(spriteName);

        if (selectedSprite != null)
        {
            cardImage.sprite = selectedSprite;
        }
        else
        {
            Debug.LogWarning($"[ViewCard] ��������Ʈ '{spriteName}'�� ã�� ���߽��ϴ�.");
        }
    }

    // �� ī�� ���̱�
    public void Show()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                Debug.LogWarning($"[ViewCard] Show() �� CanvasGroup�� ���� �ڵ� �߰���: {name}");
            }
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    // �� ī�� �����
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    // |-------------------------------------------------

    // �˾� �ؽ�Ʈ ����
    public void popupTextSetting()
    {
        viewCardText.TextUpdate(suit, rank);
    }

    public void EnterViewCard()
    {
        AnimationManager.Instance.OnEnterViewCard(gameObject);
    }

    public void ExitViewCard()
    {
        AnimationManager.Instance.OnExitViewCard(gameObject);
    }
}
