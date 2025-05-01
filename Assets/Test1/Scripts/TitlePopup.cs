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

    private void Start()
    {
        OptionPanel.SetActive(false);

        QuitPanel.SetActive(false);

        RunPanel.SetActive(false);

        SoundOptionPanel.SetActive(false);
    }

    // PlayButton 클릭 시 
    public void PlayButtonClick()
    { 
        RunPanel.SetActive(true);
    }

    public void DeletePlayClick()
    {
        RunPanel.SetActive(false);
    }

    // |---------------------------

    // OptionButton 클릭 시 
    public void OptionButtonClick()
    {
        OptionPanel.SetActive(true);
    }

    // 옵션 중 설정 클릭 시 
    public void SoundOptionClick()
    {
        OptionPanel.SetActive(false);
        SoundOptionPanel.SetActive(true);
    }

    // 사운드 설정 닫기
    public void DeleteSoundClick()
    {
        SoundOptionPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void DeleteOptionClick()
    {
        OptionPanel.SetActive(false);
    }

    // |---------------------------

    // QuitButton 클릭 시 
    public void QuitButtonClick()
    {
        QuitPanel.SetActive(true);
    }

    public void DeleteQuitClick()
    {
        QuitPanel.SetActive(false);
    }
}
