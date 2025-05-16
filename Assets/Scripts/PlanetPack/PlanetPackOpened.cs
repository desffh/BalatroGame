using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;


// ī������ ���� ��, â ���� & �༺ī���� ������ ���� & �ִϸ��̼� ����

public class PlanetPackOpened : MonoBehaviour
{
    [SerializeField] private PlanetManager planetManager;

    [SerializeField] private Image cardPackImage;

    [SerializeField] private PlanetCard [] planetCards;

    [SerializeField] private GameObject panel;
     
    [SerializeField] private Transform [] positions; // ������ ��ġ �迭

    private int count; // ī�� ���� 

    private PlanetCardPack currentPack;


    private void Start()
    {
        for(int i = 0; i < planetCards.Length; i++)
        {
            planetCards[i].gameObject.SetActive(false);
        }
    }

    public void Register(PlanetCardPack cardPack)
    {
        // �̺�Ʈ ����
        cardPack.OnPackOpened += HandlePackOpened;
    }

    private void HandlePackOpened(PlanetCardPack pack)
    {
        currentPack = pack;

        PackOpened(currentPack); // â ���� & �༺ ī�� ����
    }


    private void PackOpened(PlanetCardPack pack)
    {
        count = pack.planetPackUI.decCount;

        panel.SetActive(true);

        // ī���� �̹��� ����
        cardPackImage.sprite = pack.planetPackUI.packImage.sprite;

        // �༺ī�� ����
        planetManager.ShuffleBuffer();

        for (int i = 0; i < pack.planetPackUI.decCount; i++)
        {
            // ���� �༺ ������
            var data = planetManager.PopData();
            
            // ī�忡 ����
            planetCards[i].SetData(data);

            planetCards[i].GetComponent<Image>().sprite = data.image;

            planetCards[i].gameObject.SetActive(true);
        }

        Debug.Log("[PlanetPack] ī���� â ���µ�");
    }




    // Dotween�� Sequence�� ����Ͽ� ���� �̵�

    // ī������ ������ ����

    public void CardsMove()
    {
        int startIndex = (5 - count) / 2; // �߾� ����
        
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            int index = i;

            seq.Append(
                planetCards[index].transform.DOMove(positions[startIndex + index].position, 0.7f).
                SetEase(Ease.OutExpo) // ���� ȿ��
            );
        }
    }


    // �ǳʶٱ� ��ư
    public void Hide()
    {
        panel.SetActive(false);

        //
    }
}
