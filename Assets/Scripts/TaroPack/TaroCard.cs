using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TaroCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;

    [SerializeField] private TaroTotalData taroData;

    [SerializeField] private CanvasGroup canvasGroup;


    public event Action<TaroCard> OnCardSelected; // ī�尡 Ŭ���Ǹ� ȣ��Ǵ� �̺�Ʈ

    public event Action<TaroTotalData> OnSelectButtonClick; // �����ϱ� ��ư Ŭ�� �� ȣ��Ǵ� �̺�Ʈ 

    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }

    // ������ ����
    public void SetData(TaroTotalData data)
    {
        taroData = data;

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

        // 3. �ؽ�Ʈ ����
        OnSelectButtonClick.Invoke(taroData);

        // 4. ī�� ��Ȱ��ȭ
        gameObject.SetActive(false);


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



    public void ResetCard(Vector3 startPos)
    {
        transform.DOKill();
        transform.position = startPos;
        transform.localScale = Vector3.one; // Ŀ���� ������ �ʱ�ȭ
        gameObject.SetActive(false);
    }
}
