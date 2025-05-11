using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        // ���񽺿��� ���� ������ �޾ƿ�
        var audioService = ServiceLocator.Get<IAudioService>() as SoundManager;

        if (audioService != null)
        {
            // �����̴� �ʱⰪ�� ���� ���� ���·� ����
            bgmSlider.value = audioService.bgmSource.volume;
            sfxSlider.value = audioService.sfxSource.volume;

            // �� ���� �� ���� ����
            bgmSlider.onValueChanged.AddListener(audioService.SetBGMVolume);
            sfxSlider.onValueChanged.AddListener(audioService.SetSFXVolume);
        }
        else
        {
            Debug.Log("ã�� �� ����");
        }
    }
}