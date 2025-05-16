using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Ÿ��ī���� UI & �˾�

public class TaroPackUI : MonoBehaviour
{
    private TaroCardPackSO packData; // Ÿ��ī�� �� ������

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

    // Ÿ�� ī�� �� ������ ����
    public void Init(TaroCardPackSO data)
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


    // Ÿ�� ī�� �� �˾� ������ ����
    public void Initialize(string name, int dec, int select, int cost)
    {
        nameText.text = $"<color=#FF0000>{name}</color>";

        infoText.text = $"<color=#0000FF>{dec}</color>���� Ÿ�� ī�� �� �ִ� <color=#0000FF>{select}��</color>�� ������ ����մϴ�.";

        costText.text = $"<color=#FFA500>${cost}</color>";
    }

    // �̺�Ʈ Ʈ���� - Ÿ�� ī�� �� �ִϸ��̼�

    // �̺�Ʈ Ʈ���� Enter
    public void OnEnterTaro()
    {
        AnimationManager.Instance.OnEnterShopCard(gameObject);
    }

    // �̺�Ʈ Ʈ���� Exit
    public void OnExitTaro()
    {
        AnimationManager.Instance.OnExitShopCard(gameObject);
    }
}
