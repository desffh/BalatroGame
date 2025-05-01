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

    private void Start()
    {
        OptionPanel.SetActive(false);

        QuitPanel.SetActive(false);

        RunPanel.SetActive(false);

        SoundOptionPanel.SetActive(false);
    }

    // PlayButton Ŭ�� �� 
    public void PlayButtonClick()
    { 
        RunPanel.SetActive(true);
    }

    public void DeletePlayClick()
    {
        RunPanel.SetActive(false);
    }

    // |---------------------------

    // OptionButton Ŭ�� �� 
    public void OptionButtonClick()
    {
        OptionPanel.SetActive(true);
    }

    // �ɼ� �� ���� Ŭ�� �� 
    public void SoundOptionClick()
    {
        OptionPanel.SetActive(false);
        SoundOptionPanel.SetActive(true);
    }

    // ���� ���� �ݱ�
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

    // QuitButton Ŭ�� �� 
    public void QuitButtonClick()
    {
        QuitPanel.SetActive(true);
    }

    public void DeleteQuitClick()
    {
        QuitPanel.SetActive(false);
    }
}
