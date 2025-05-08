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
        // 서비스에서 현재 볼륨을 받아옴
        var audioService = ServiceLocator.Get<IAudioService>() as SoundManager;

        if (audioService != null)
        {
            // 슬라이더 초기값을 현재 사운드 상태로 설정
            bgmSlider.value = audioService.bgmSource.volume;
            sfxSlider.value = audioService.sfxSource.volume;

            // 값 변경 시 볼륨 설정
            bgmSlider.onValueChanged.AddListener(audioService.SetBGMVolume);
            sfxSlider.onValueChanged.AddListener(audioService.SetSFXVolume);
        }
        else
        {
            Debug.Log("찾을 수 없음");
        }
    }
}