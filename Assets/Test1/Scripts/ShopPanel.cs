using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    private void OnEnable()
    {
        var parent = GetComponentInParent<Shop>();
        if (parent != null)
        {
            parent.GenerateShopJokers();
        }
    }
}
