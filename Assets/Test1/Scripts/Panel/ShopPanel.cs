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
        yield return null; // ���� �����ӱ��� ��ٸ�

        OnShopOpened?.Invoke(); // ��� UI ������Ʈ���� Start() ���� �� ��!
    }
}
