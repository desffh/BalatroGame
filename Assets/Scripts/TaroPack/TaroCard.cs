using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaroEffect
{
    public string requireSuit; // ����
    public int requireNumber;  // ���� 1 ~ 14
    public int chipBonus = 30; // Ĩ ��

    public bool IsMatch(Card card)
    {
        return card.itemdata.suit == requireSuit && card.itemdata.id == requireNumber;
    }
}


public class TaroCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    [SerializeField] private TaroTotalData taroData;

    [SerializeField] private CanvasGroup canvasGroup;


    public event Action<TaroCard> OnCardSelected; // ī�尡 Ŭ���Ǹ� ȣ��Ǵ� �̺�Ʈ

    public event Action<TaroCard> OnSelectButtonClick; // �����ϱ� ��ư Ŭ�� �� ȣ��Ǵ� �̺�Ʈ 



    public int RandomNumber { get; private set; }


    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }

    // ������ ����
    public void SetData(TaroTotalData data)
    {
        taroData = data;

        RandomNumber = UnityEngine.Random.Range(2, 15);

        if (cardImage != null && data != null)
        {
            cardImage.sprite = data.image;
        }
    }


    // ����ϱ� ��ư Ŭ�� ��
    public void OnButtonClick()
    {
        if (taroData == null)
        {
            Debug.Log("Ÿ�� ī�� ������ ����");
            return;
        }

        // 0. ī�� ��ȣ�ۿ� ����
        OffRaycast();

        gameObject.GetComponent<RunPopUp>().OnExit();


        // 2. �ɷ� ����
        PokerManager.Instance.AddTaroEffect(taroData, RandomNumber);

        // 3. �ؽ�Ʈ ����
        OnSelectButtonClick.Invoke(this);

        // 4. ī�� ��Ȱ��ȭ
        gameObject.SetActive(false);


    }

    // ���� ���� ��ȯ
    public int GetRandomNumber()
    {
        return RandomNumber;
    }

    // ī�� Ŭ�� �� ȣ�� (�̺�Ʈ Ʈ����)
    public void OnClickCard()
    {
        OnCardSelected?.Invoke(this);
    }


    public void OffRaycast()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public void OnRaycast()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public TaroTotalData GetData()
    {
        return taroData;
    }


    public void ResetCard(Vector3 startPos)
    {
        transform.DOKill();
        transform.position = startPos;
        transform.localScale = Vector3.one; // Ŀ���� ������ �ʱ�ȭ
        RandomNumber = 0;
        gameObject.SetActive(false);
    }
}
