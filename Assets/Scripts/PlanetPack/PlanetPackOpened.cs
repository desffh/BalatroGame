using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;


// 카드팩이 열릴 때, 창 열기 & 행성카드의 데이터 세팅 & 애니메이션 실행

public class PlanetPackOpened : MonoBehaviour
{
    [SerializeField] private PlanetManager planetManager;

    [SerializeField] private Image cardPackImage;

    [SerializeField] private PlanetCard [] planetCards;

    [SerializeField] private GameObject panel;
     
    [SerializeField] private Transform [] positions; // 생성될 위치 배열

    private int count; // 카드 갯수 

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
        // 이벤트 구독
        cardPack.OnPackOpened += HandlePackOpened;
    }

    private void HandlePackOpened(PlanetCardPack pack)
    {
        currentPack = pack;

        PackOpened(currentPack); // 창 열기 & 행성 카드 세팅
    }


    private void PackOpened(PlanetCardPack pack)
    {
        count = pack.planetPackUI.decCount;

        panel.SetActive(true);

        // 카드팩 이미지 설정
        cardPackImage.sprite = pack.planetPackUI.packImage.sprite;

        // 행성카드 설정
        planetManager.ShuffleBuffer();

        for (int i = 0; i < pack.planetPackUI.decCount; i++)
        {
            // 뽑은 행성 데이터
            var data = planetManager.PopData();
            
            // 카드에 주입
            planetCards[i].SetData(data);

            planetCards[i].GetComponent<Image>().sprite = data.image;

            planetCards[i].gameObject.SetActive(true);
        }

        Debug.Log("[PlanetPack] 카드팩 창 오픈됨");
    }




    // Dotween의 Sequence를 사용하여 순차 이동

    // 카드팩을 누르면 실행

    public void CardsMove()
    {
        int startIndex = (5 - count) / 2; // 중앙 정렬
        
        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < count; i++)
        {
            int index = i;

            seq.Append(
                planetCards[index].transform.DOMove(positions[startIndex + index].position, 0.7f).
                SetEase(Ease.OutExpo) // 감속 효과
            );
        }
    }


    // 건너뛰기 버튼
    public void Hide()
    {
        panel.SetActive(false);

        //
    }
}
