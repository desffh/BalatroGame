using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// 행성카드팩 UI & 팝업

public class PlanetPackUI : MonoBehaviour 
{
    private PlanetCardPackSO packData; // 행성카드 팩 데이터
    
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


    // 행성 카드 팩 데이터 세팅
    public void Init(PlanetCardPackSO data)
    {
        packData = data;

        packName = data.packName;

        packCost = data.cost;

        decCount = data.decCount;

        selectCount = data.selectCount;

        // 랜덤 이미지 선택
        if (data.planetPackImages.Count > 0)
        {
            var randomSprite = data.planetPackImages[Random.Range(0, data.planetPackImages.Count)];

            packImage.sprite = randomSprite;
        }

       
        Initialize(packName, decCount, selectCount, packCost);
    }


    // 행성 카드 팩 팝업 데이터 세팅
    public void Initialize(string name, int dec, int select, int cost)
    {
        nameText.text = $"<color=#FF0000>{name}</color>";

        infoText.text = $"<color=#0000FF>{dec}</color>장의 행성 카드 중 최대 <color=#0000FF>{select}장</color>을 선택해 사용합니다.";

        costText.text = $"<color=#FFA500>${cost}</color>";
    }

    // 이벤트 트리거 - 행성 카드 팩 애니메이션
    // 이벤트 트리거 Enter
    public void OnEnterPlanet()
    {
        AnimationManager.Instance.OnEnterShopCard(gameObject);
    }

    // 이벤트 트리거 Exit
    public void OnExitPlanet()
    {
        AnimationManager.Instance.OnExitShopCard(gameObject);
    }

}
