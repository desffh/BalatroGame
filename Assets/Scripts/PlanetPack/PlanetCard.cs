using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlanetCard : MonoBehaviour
{
    [SerializeField] private Image cardImage; // ī�忡 ǥ�õ� �̹���

    [SerializeField] private PlanetTotalData planetData;

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


    public void OnClick()
    {
        if (planetData == null)
        {
            Debug.LogWarning("PlanetCard�� �����Ͱ� �����ϴ�.");
            return;
        }

        // 1. �༺ ȿ�� ����
        //ApplyEffect(planetData);

        // 2. ���õ��� ���� ī����� ���ۿ� �ǵ�����
        //PlanetPackOpened.Instance.ReturnUnusedCardsExcept(planetData);

        // 3. â �ݱ�
        //PlanetPackOpened.Instance.Hide();

    }


    // |----


    // �̺�Ʈ Ʈ���� Enter
    public void OnEnterPlanet()
    {
        AnimationManager.Instance.OnEnterShopCard(gameObject);
    }

    // �̺�Ʈ Ʈ���� Exit
    public void OnExitPlanet()
    {
        AnimationManager.Instance.OnExitShopCard(gameObject);
    }
}
