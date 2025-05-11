using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


// UI ����� Ȯ�ο�
public class UIRaycastDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UI�� ���� ��� ������Ʈ�� ���� ����Ʈ ����
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
               // Debug.Log("���콺�� ���� UI ������Ʈ��:");
                foreach (RaycastResult r in results)
                {
                 //   Debug.Log($"- {r.gameObject.name}");
                }
            }
            else
            {
               // Debug.Log("UI�� ���� ������Ʈ ����");
            }
        }
    }
}
