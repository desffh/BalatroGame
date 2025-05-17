using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaroCardClick : MonoBehaviour
{
    // 이벤트 트리거 Enter
    public void OnEnterTaro()
    {
        AnimationManager.Instance.OnEnterShopCard(gameObject);
    }

    // 이벤트 트리거 Exit
    public void OnExitTaro()
    {
        AnimationManager.Instance.OnExitShopCard(gameObject);
    }
}
