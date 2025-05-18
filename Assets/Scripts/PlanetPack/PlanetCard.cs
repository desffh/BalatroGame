using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PlanetCard : MonoBehaviour
{
    [SerializeField] private Image cardImage; // ī�忡 ǥ�õ� �̹���

    [SerializeField] private PlanetTotalData planetData;

    [SerializeField] private CanvasGroup canvasGroup;

    
    public event Action<PlanetCard> OnCardSelected; // ī�尡 Ŭ���Ǹ� ȣ��Ǵ� �̺�Ʈ

    public event Action<PlanetTotalData> OnSelectButtonClick; // �����ϱ� ��ư Ŭ�� �� ȣ��Ǵ� �̺�Ʈ 

    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }



    // ������ ����
    public void SetData(PlanetTotalData data)
    {
        planetData = data;

        if (cardImage != null && data != null)
        {
            cardImage.sprite = data.image;

            
        }
    }


    // ����ϱ� ��ư Ŭ�� �� ���� ���� ���׷��̵�
    public void OnButtonClick()
    {
        if (planetData == null)
        {
            Debug.Log("�༺ ī�� ������ ����");
            return;
        }
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ClickCard");

        // selectCount�� ���� ���Ͽ� �� �����ؾ��ϴ� ��, �����ؾ��ϴ� �� �Ǻ�


        // 0. ī�� ��ȣ�ۿ� ����
        OffRaycast();

        gameObject.GetComponent<RunPopUp>().OnExit();

        // 1. ��ġ �״�� (�ӽ�)

        // 2. ���׷��̵� ����
        PokerManager.Instance.ApplyPlanetUpgrade(planetData);

        // 3. �ؽ�Ʈ ����
        OnSelectButtonClick.Invoke(planetData);

        // 4. ī�� ��Ȱ��ȭ
        gameObject.SetActive(false);

        // 5. �� ���� �ؽ�Ʈ ����

        //Debug.Log("Ŭ�����Դϴ� �༺");
    }


    // �̺�Ʈ Ʈ����
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


    // |----



}
