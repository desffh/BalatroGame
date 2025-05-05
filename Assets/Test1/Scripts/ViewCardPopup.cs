using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCardPopup : MonoBehaviour
{
    [SerializeField] GameObject viewCardPopup;

    private void Start()
    {
        viewCardPopup.SetActive(false);
    }

    public void OnEnterMouse()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-EnterCard");

        viewCardPopup.SetActive(true);
    }

    public void OnLeaveMouse()
    {
        viewCardPopup.SetActive(false);
    }
}
