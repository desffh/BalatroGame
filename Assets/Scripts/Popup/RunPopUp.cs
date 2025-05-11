using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunPopUp : MonoBehaviour
{
    [SerializeField] GameObject Popup;

    private void Start()
    {
        Popup.SetActive(false);
    }

    public void OnEnter()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-EnterCard");

        Popup.SetActive(true);
    }

    public void OnExit()
    {
        Popup.SetActive(false);
    }
}
