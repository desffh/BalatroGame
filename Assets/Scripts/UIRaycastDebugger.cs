using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


// UI 디버그 확인용
public class UIRaycastDebugger : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UI에 닿은 모든 오브젝트를 담을 리스트 생성
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
               // Debug.Log("마우스가 닿은 UI 오브젝트들:");
                foreach (RaycastResult r in results)
                {
                 //   Debug.Log($"- {r.gameObject.name}");
                }
            }
            else
            {
               // Debug.Log("UI에 닿은 오브젝트 없음");
            }
        }
    }
}
