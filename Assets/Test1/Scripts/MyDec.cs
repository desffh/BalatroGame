using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyDec : MonoBehaviour
{
    [SerializeField] GameObject myDecText;

    private void Start()
    {
        myDecText.SetActive(false);
    }

    public void OnEnterCard()
    {
        myDecText.SetActive(true);
    }

    public void OffEnterCard()
    {
        myDecText.SetActive(false);
    }
}
