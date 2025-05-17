using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCardClick : MonoBehaviour
{
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
