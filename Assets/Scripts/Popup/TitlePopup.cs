using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타이틀 씬에 있는 모든 팝업 관리 (팝업 활성화 / 비활성화 ) 담당
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

    // PlayButton 클릭 시 RunPanel 열기 
    public void PlayButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");
        RunPanel.SetActive(true);
    }

    // RunPanel 닫기
    public void DeletePlayClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        RunPanel.SetActive(false);
    }

    // |---------------------------

    // OptionButton 클릭 시 
    public void OptionButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OptionPanel.SetActive(true);
    }

    // 옵션 - 설정 클릭 시 
    public void SoundOptionClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OptionPanel.SetActive(false);
        SoundOptionPanel.SetActive(true);
    }

    // 사운드 설정 닫기
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

    // QuitButton 클릭 시 
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
    // GuideButton 클릭 시 
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


    // 크레딧 버튼

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
