using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// �� ����, �ɼ�, �� ī�� ����Ʈ, Ŭ���� â
public class ScreenPopupManager : MonoBehaviour
{
    [SerializeField] PanelHandler RunPanel;

    [SerializeField] PanelHandler OptionPanel;

    [SerializeField] PanelHandler ViewCardsPanel;

    [SerializeField] PanelHandler clearPanel;

    [SerializeField] PanelHandler SoundOptionPanel;

    [SerializeField] TextMeshProUGUI pointText;
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

    // �� ���� Ŭ�� �� ����
    public void RunOnClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(RunPanel);

    }

    // �� ���� �ݱ�
    public void RunDeleteClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(RunPanel);

    }
    
    // |----------------------------------

    // �ɼ� ��ư Ŭ�� �� ����
    public void OptionButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(OptionPanel);
    }

    // �ɼ� �ݱ�
    public void DeleteOptionClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(OptionPanel);
    }

    // |----------------------------------

    // ������ ī�� Ŭ�� �� ����
    public void ViewCardClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(ViewCardsPanel);
    }

    // ������ ī�� �ݱ�
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

        yield return null; // ���� �����ӿ�
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

    // |--------------------------------------

    // �˾� �ִϸ��̼� �ݱ�
    public void OnCloseButtonClick(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // ���� Ʈ�� ����

        popupWindow.Hide();

    }

    // �˾� �ִϸ��̼� ����
    public void OnButtonClick(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // ���� Ʈ�� ����

        popupWindow.gameObject.SetActive(true); // �ݵ�� ���� Ȱ��ȭ

        popupWindow.Show();

    }

    // clear �г� ȣ�� + �ؽ�Ʈ �ִϸ��̼�
    public void EndPanelShow(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // ���� Ʈ�� ����

        popupWindow.gameObject.SetActive(true); // �ݵ�� ���� Ȱ��ȭ

        popupWindow.onPanelShowComplete = () =>
        {
            AnimationManager.Instance.TMProText(pointText, 0.7f);
            
        };

        popupWindow.EndPanelShow();
    }
}
