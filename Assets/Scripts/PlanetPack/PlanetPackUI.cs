using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// ����ü ����� -> ������ Name, cost, Sprite 

// ����ü �迭�����, ������ �����տ� ������ �ֱ�

public class PlanetPackUI : MonoBehaviour 
{
    [SerializeField] private Image packImage;

    public string packName;

    public int packCost;


    private PlanetCardPackSO packData; // �༺ī�� �� ������

    // �༺ ī�� �� ������ ����
    public void Init(PlanetCardPackSO data)
    {
        packData = data;

        packName = data.packName;

        packCost = data.cost;


        // ���� �̹��� ����
        if (data.planetPackImages.Count > 0)
        {
            var randomSprite = data.planetPackImages[Random.Range(0, data.planetPackImages.Count)];

            packImage.sprite = randomSprite;
        }
    }

    public void OnClicked()
    {
        // ���� �˾� �Ǵ� ī�� ���� â ����
        
    }
}
