using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardPopup : MonoBehaviour
{
    [SerializeField] GameObject popup;

    [SerializeField] public TextMeshPro suitText;
    [SerializeField] public TextMeshPro rankText;


    private void Start()
    {
        popup.SetActive(false);
    }

    public void MouseEnter()
    {
        popup.SetActive(true);
    }

    public void MouseExit()
    {
        popup.SetActive(false);
    }
}
