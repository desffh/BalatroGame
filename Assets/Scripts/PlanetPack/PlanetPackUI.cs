using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// �༺ī���� UI & �˾�

public class PlanetPackUI : MonoBehaviour 
{
    private PlanetCardPackSO packData; // �༺ī�� �� ������
    
    public string packName;
    
    public int packCost;

    public int decCount;

    public int selectCount;

    
    // |----

    
    [SerializeField] Image packImage;

    [SerializeField] TextMeshProUGUI nameText;

    [SerializeField] TextMeshProUGUI infoText;

    [SerializeField] TextMeshProUGUI costText; 


    // |----


    // �༺ ī�� �� ������ ����
    public void Init(PlanetCardPackSO data)
    {
        packData = data;

        packName = data.packName;

        packCost = data.cost;

        decCount = data.decCount;

        selectCount = data.selectCount;

        // ���� �̹��� ����
        if (data.planetPackImages.Count > 0)
        {
            var randomSprite = data.planetPackImages[Random.Range(0, data.planetPackImages.Count)];

            packImage.sprite = randomSprite;
        }

       
        Initialize(packName, decCount, selectCount, packCost);
    }


    // �༺ ī�� �� �˾� ������ ����
    public void Initialize(string name, int dec, int select, int cost)
    {
        nameText.text = $"<color=#FF0000>{name}</color>";

        infoText.text = $"<color=#0000FF>{dec}</color>���� �༺ ī�� �� �ִ� <color=#0000FF>{select}��</color>�� ������ ����մϴ�.";

        costText.text = $"<color=#FFA500>${cost}</color>";
    }

    // �̺�Ʈ Ʈ���� - �༺ ī�� �� �ִϸ��̼�
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
