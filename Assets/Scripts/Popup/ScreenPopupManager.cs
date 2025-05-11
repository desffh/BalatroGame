using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// 런 정보, 옵션, 뷰 카드 리스트, 클리어 창
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
        RunPanel.transform.localScale = Vector3.one * 0.2f; // 처음 크기 설정
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

    // 런 정보 클릭 시 열기
    public void RunOnClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(RunPanel);

    }

    // 런 정보 닫기
    public void RunDeleteClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(RunPanel);

    }
    
    // |----------------------------------

    // 옵션 버튼 클릭 시 열기
    public void OptionButtonClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(OptionPanel);
    }

    // 옵션 닫기
    public void DeleteOptionClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(OptionPanel);
    }

    // |----------------------------------

    // 프리뷰 카드 클릭 시 열기
    public void ViewCardClick()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnButtonClick(ViewCardsPanel);
    }

    // 프리뷰 카드 닫기
    public void DeleteViewCard()
    {
        ServiceLocator.Get<IAudioService>().PlaySFX("Sound-ButtonClick");

        OnCloseButtonClick(ViewCardsPanel);
    }

    // |----------------------------------

    // 스테이지 클리어 시
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

        yield return null; // 다음 프레임에
    }

    // |----------------------------------

    // 옵션 > 설정 클릭 시
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

    // 팝업 애니메이션 닫기
    public void OnCloseButtonClick(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // 기존 트윈 제거

        popupWindow.Hide();

    }

    // 팝업 애니메이션 열기
    public void OnButtonClick(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // 기존 트윈 제거

        popupWindow.gameObject.SetActive(true); // 반드시 먼저 활성화

        popupWindow.Show();

    }

    // clear 패널 호출 + 텍스트 애니메이션
    public void EndPanelShow(PanelHandler popupWindow)
    {
        popupWindow.transform.DOKill(); // 기존 트윈 제거

        popupWindow.gameObject.SetActive(true); // 반드시 먼저 활성화

        popupWindow.onPanelShowComplete = () =>
        {
            AnimationManager.Instance.TMProText(pointText, 0.7f);
            
        };

        popupWindow.EndPanelShow();
    }
}
