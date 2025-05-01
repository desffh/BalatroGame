using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 런 정보, 옵션, 뷰 카드 리스트, 클리어 창
public class ScreenPopupManager : MonoBehaviour
{
    [SerializeField] GameObject RunPanel;

    [SerializeField] GameObject OptionPanel;

    [SerializeField] GameObject ViewCardsPanel;

    [SerializeField] GameObject clearPanel;
    // |----------------------------------

    private void Start()
    {
        RunPanel.SetActive(false);

        OptionPanel.SetActive(false);

        ViewCardsPanel.SetActive(false);
        
        clearPanel.SetActive(false);
    }

    // |----------------------------------

    // 런 정보 클릭 시 
    public void RunOnClick()
    {
        SoundManager.Instance.ButtonClick();
        RunPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void RunDeleteClick()
    {
        RunPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    
    // |----------------------------------

    // 옵션 버튼 클릭 시 
    public void OptionButtonClick()
    {
        OptionPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void DeleteOptionClick()
    {
        OptionPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // |----------------------------------

    // 프리뷰 카드 클릭 시 
    public void ViewCardClick()
    {
        ViewCardsPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void DeleteViewCard()
    {
        ViewCardsPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    // |----------------------------------

    // 스테이지 클리어 시
    public void onClearPanel()
    {
        clearPanel.SetActive(true);
    }

    public IEnumerator OnClearPanel()
    {
        onClearPanel();

        yield return null;
    }
}
