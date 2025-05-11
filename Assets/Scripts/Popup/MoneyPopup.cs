using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPopup : MonoBehaviour
{
    [SerializeField] GameObject moneyPopup;

    private void Start()
    {
        moneyPopup.SetActive(false);
    }

    public void OnEnter()
    {
        moneyPopup.SetActive(true);
    }

    public void OnExit()
    {
        moneyPopup.SetActive(false);
    }
}
