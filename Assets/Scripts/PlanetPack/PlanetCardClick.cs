using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCardClick : MonoBehaviour
{
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
