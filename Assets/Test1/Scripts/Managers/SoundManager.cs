using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    // |-------------------------------------

    [Header("BGM Clips")]
    public AudioClip titleBGM;
    public AudioClip gameBGM;
    public AudioClip shopBGM;
    public AudioClip bossBGM;


    // |-------------------------------------


    [Header("SFX Clips")]
    public AudioClip cardEnter;
    public AudioClip cardSpawn;
    public AudioClip cardClick;
    public AudioClip checkCard;
    public AudioClip buttonClick;
    public AudioClip failure;

    // |-------------------------------------

    private Coroutine fadeCoroutine;

    protected override void Awake()
    {
        base.Awake();

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
    }

    protected override void InitializeReferences()
    {
        PlaySceneBGM(SceneManager.GetActiveScene().name);
    }

    public void PlaySceneBGM(string sceneName)
    {
        switch (sceneName)
        {
            case "TitleScene":
                PlayBGM(titleBGM);
                break;
            case "StageScene":
                PlayBGM(gameBGM);
                break;
            default:
                StopBGM();
                break;
        }
    }

    // 일반 게임 BGM 재생
    public void PlayGameBGM()
    {
        PlayBGM(gameBGM);
    }

    // BGM 재생
    public void PlayBGMs(AudioClip Clip)
    {
        PlayBGM(Clip);
    }



    private void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOutInBGM(clip));
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // |-------------------------------------
    private IEnumerator FadeOutInBGM(AudioClip newClip)
    {
        float fadeDuration = 0.05f;  // 페이드 시간 (초 단위)
        float startVolume = 0.3f;

        // 1. 현재 BGM 페이드 아웃
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.volume = 0f;

        bgmSource.Play();  // 새 Clip 처음부터 Play

        // 2. 새 BGM 페이드 인
        while (bgmSource.volume < startVolume)
        {
            bgmSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        bgmSource.volume = startVolume;  // 정확히 맞추기
    }

    // |-------------------------------------

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // 카드에 닿았을 때 사용하는 함수
    public void PlayCardEnter()
    {
        //Debug.Log("효과음 재생");
        PlaySFX(cardEnter);
    }

    // 카드 나눠줄 때 사용하는 함수
    public void PlayCardSpawn()
    {
        PlaySFX(cardSpawn);
    }

    // 카드를 클릭했을 때 호출하는 함수
    public void PlayCardClick()
    {
        PlaySFX(cardClick);
    }

    // 카드가 계산될 때 호출되는 함수
    public void CheckCard()
    {
        PlaySFX(checkCard);
    }

    // 보스 스테이지 시작
    public void OnBossStageStart()
    {
        PlayBGMs(bossBGM);
    }

    // 보스 클리어
    public void OnBossStageClear()
    {
        PlayGameBGM();
    }

    // 상점 스테이지 시작
    public void OnShopStart()
    {
        PlayBGMs(shopBGM);
    }

    public void ButtonClick()
    {
        PlaySFX(buttonClick);
    }

    public void StageFail()
    {
        PlaySFX(failure);
    }

    public void PlayCardCountSFX()
    {
        if (checkCard != null)  // 카드 세는 소리 클립
        {
            sfxSource.PlayOneShot(checkCard);
        }
    }

    public void ResetSFXPitch()
    {
        sfxSource.pitch = 1.0f;
    }

}
