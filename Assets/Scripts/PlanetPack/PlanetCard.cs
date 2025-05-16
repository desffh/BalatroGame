using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlanetCard : MonoBehaviour
{
    [SerializeField] private Image cardImage; // 카드에 표시될 이미지

    [SerializeField] private PlanetTotalData planetData;

    private void Awake()
    {
        cardImage = GetComponent<Image>();
    }


    // 데이터 주입
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
            Debug.LogWarning("PlanetCard에 데이터가 없습니다.");
            return;
        }

        // 1. 행성 효과 적용
        //ApplyEffect(planetData);

        // 2. 선택되지 않은 카드들은 버퍼에 되돌리기
        //PlanetPackOpened.Instance.ReturnUnusedCardsExcept(planetData);

        // 3. 창 닫기
        //PlanetPackOpened.Instance.Hide();

    }


    // |----


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
