using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


// 구조체 만들기 -> 데이터 Name, cost, Sprite 

// 구조체 배열만들고, 각각의 프리팹에 데이터 넣기

public class PlanetPackUI : MonoBehaviour 
{
    [SerializeField] private Image packImage;

    public string packName;

    public int packCost;


    private PlanetCardPackSO packData; // 행성카드 팩 데이터

    // 행성 카드 팩 데이터 세팅
    public void Init(PlanetCardPackSO data)
    {
        packData = data;

        packName = data.packName;

        packCost = data.cost;


        // 랜덤 이미지 선택
        if (data.planetPackImages.Count > 0)
        {
            var randomSprite = data.planetPackImages[Random.Range(0, data.planetPackImages.Count)];

            packImage.sprite = randomSprite;
        }
    }

    public void OnClicked()
    {
        // 구매 팝업 또는 카드 선택 창 열기
        
    }
}
