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
            // 상점이 활성화되면 실행 할 함수 호출
            //parent.
        }
    }
}
