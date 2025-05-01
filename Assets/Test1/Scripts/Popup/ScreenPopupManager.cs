using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �� ����, �ɼ�, �� ī�� ����Ʈ, Ŭ���� â
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

    // �� ���� Ŭ�� �� 
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

    // �ɼ� ��ư Ŭ�� �� 
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

    // ������ ī�� Ŭ�� �� 
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

    // �������� Ŭ���� ��
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
