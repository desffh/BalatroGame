using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �� ����, �ɼ�, �� ī�� ����Ʈ, Ŭ���� â
public class ScreenPopupManager : MonoBehaviour
{
    [SerializeField] PanelHandler RunPanel;

    [SerializeField] PanelHandler OptionPanel;

    [SerializeField] PanelHandler ViewCardsPanel;

    [SerializeField] PanelHandler clearPanel;

    [SerializeField] PanelHandler SoundOptionPanel;
    // |----------------------------------

    private void Start()
    {
        RunPanel.transform.localScale = Vector3.one * 0.2f; // ó�� ũ�� ����
        OptionPanel.transform.localScale = Vector3.one * 0.2f;
        ViewCardsPanel.transform.localScale = Vector3.one * 0.2f;
        SoundOptionPanel.transform.localScale = Vector3.one * 0.2f;

        RunPanel.gameObject.SetActive(false);
        OptionPanel.gameObject.SetActive(false);
        ViewCardsPanel.gameObject.SetActive(false);
        SoundOptionPanel.gameObject.SetActive(false);

        clearPanel.gameObject.SetActive(false);
    }

    // |----------------------------------

    // �� ���� Ŭ�� �� 
    public void RunOnClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(RunPanel);

    }

    public void RunDeleteClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(RunPanel);

    }
    
    // |----------------------------------

    // �ɼ� ��ư Ŭ�� �� 
    public void OptionButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(OptionPanel);
    }

    public void DeleteOptionClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(OptionPanel);
    }

    // |----------------------------------

    // ������ ī�� Ŭ�� �� 
    public void ViewCardClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(ViewCardsPanel);
    }

    public void DeleteViewCard()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(ViewCardsPanel);
    }

    // |----------------------------------

    // �������� Ŭ���� ��
    public void onClearPanel()
    {
        EndPanelShow(clearPanel);
    }

    public void ClearPanel()
    {
        clearPanel.gameObject.SetActive(true);
    }

    public IEnumerator OnClearPanel()
    {
        onClearPanel();

        yield return null;
    }

    // |----------------------------------

    // �ɼ� > ���� Ŭ�� ��
    public void OnSoundOptionPanel()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(SoundOptionPanel);
    }

    public void DeleteSoundOption()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(SoundOptionPanel);

    }


    public void OnCloseButtonClick(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // ���� Ʈ�� ����

        popupWindow.Hide();

    }

    public void OnButtonClick(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // ���� Ʈ�� ����

        popupWindow.gameObject.SetActive(true); // �ݵ�� ���� Ȱ��ȭ

        popupWindow.Show();

    }

    public void EndPanelShow(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // ���� Ʈ�� ����

        popupWindow.gameObject.SetActive(true); // �ݵ�� ���� Ȱ��ȭ

        popupWindow.EndPanelShow();
    }
}
