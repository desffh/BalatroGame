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
            // ������ Ȱ��ȭ�Ǹ� ���� �� �Լ� ȣ��
            //parent.
        }
    }
}
