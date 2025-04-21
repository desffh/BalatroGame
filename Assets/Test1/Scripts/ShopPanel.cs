using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    public static event System.Action OnShopOpened;

    private void OnEnable()
    {
        StartCoroutine(DelayedOpen());
    }

    IEnumerator DelayedOpen()
    {
        yield return null; // 다음 프레임까지 기다림

        OnShopOpened?.Invoke(); // 모든 UI 컴포넌트들이 Start() 돌고 난 뒤!
    }
}
