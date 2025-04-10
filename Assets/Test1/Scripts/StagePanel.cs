using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePanel : MonoBehaviour
{
    [SerializeField] GameObject stagePanel;

    private void Awake()
    {
        stagePanel = GetComponent<GameObject>();
    }


}
