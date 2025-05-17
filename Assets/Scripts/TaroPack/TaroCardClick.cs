using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaroCardClick : MonoBehaviour
{
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
