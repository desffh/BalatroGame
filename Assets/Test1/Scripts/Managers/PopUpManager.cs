using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopUpManager : Singleton<PopUpManager>
{
    [SerializeField] GameObject clearPopUp;

    

    protected override void Awake()
    {
        base.Awake();

        clearPopUp.SetActive(false);
    }

    public void OnClearPopUp()
    {
        clearPopUp.SetActive(true);
    }

    public IEnumerator OnClearPopup()
    {
        OnClearPopUp();
        
        yield return null;
    }
    
}
