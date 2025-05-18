using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ÿ��Ʋ ���� �ִ� ��� �˾� ���� (�˾� Ȱ��ȭ / ��Ȱ��ȭ ) ���
public class TitlePopup : MonoBehaviour
{
    [SerializeField] GameObject OptionPanel;

    [SerializeField] GameObject QuitPanel;

    [SerializeField] GameObject RunPanel;

    [SerializeField] GameObject SoundOptionPanel;

    [SerializeField] GameObject GuidePanel;

    [SerializeField] GameObject CreditPanel;


    private void Start()
    {
        OptionPanel.SetActive(false);

        QuitPanel.SetActive(false);

        RunPanel.SetActive(false);

        SoundOptionPanel.SetActive(false);

        GuidePanel.SetActive(false);
    }

    // PlayButton Ŭ�� �� RunPanel ���� 
    public void PlayButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");
        RunPanel.SetActive(true);
    }

    // RunPanel �ݱ�
    public void DeletePlayClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        RunPanel.SetActive(false);
    }

    // |---------------------------

    // OptionButton Ŭ�� �� 
    public void OptionButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OptionPanel.SetActive(true);
    }

    // �ɼ� - ���� Ŭ�� �� 
    public void SoundOptionClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OptionPanel.SetActive(false);
        SoundOptionPanel.SetActive(true);
    }

    // ���� ���� �ݱ�
    public void DeleteSoundClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        SoundOptionPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void DeleteOptionClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OptionPanel.SetActive(false);
    }

    // |---------------------------

    // QuitButton Ŭ�� �� 
    public void QuitButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        QuitPanel.SetActive(true);
    }

    public void DeleteQuitClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        QuitPanel.SetActive(false);
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

    }

    // |---------------------------
    // GuideButton Ŭ�� �� 
    public void GuideButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        GuidePanel.SetActive(true);
    }


    public void DeleteGuideClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        GuidePanel.SetActive(false);
    }


    // ũ���� ��ư

    public void CreditButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        CreditPanel.SetActive(true);
    }


    public void DeleteCreditClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        CreditPanel.SetActive(false);
    }
}
