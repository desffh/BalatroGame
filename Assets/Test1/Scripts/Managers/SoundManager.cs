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

    // |-------------------------------------

    private AudioClip previousClip;    // 전에 재생되던 BGM
    private float previousTime;        // 전에 재생되던 시간
    private bool hasPrevious = false;  // 복귀 가능한지 여부

    // |-------------------------------------

    [Header("Volume Settings")]
    public float bgmDefaultVolume = 0.3f;  



    protected override void Awake()
    {
        base.Awake();

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        bgmSource.volume = bgmDefaultVolume;
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
    public void PlayBGMs(AudioClip clip, float fadeDuration = 0.5f)
    {
        PlayBGM(clip, fadeDuration);
    }




    private void PlayBGM(AudioClip clip, float fadeDuration = 0.5f)
    {
        if (clip == null) return;

        if (bgmSource.clip == clip)
        {
            if (!bgmSource.isPlaying)
            {
                bgmSource.Play();
            }

            return;
        }

        if (bgmSource.isPlaying)
        {
            previousClip = bgmSource.clip;
            previousTime = bgmSource.time;
            hasPrevious = true;
        }

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOutInBGM(clip, fadeDuration)); // 여기!
    }





    public void ResumePreviousBGM()
    {
        if (hasPrevious && previousClip != null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOutInBGM(previousClip, 0.5f)); // 복귀도 부드럽게
        }
    }



    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // |-------------------------------------
    private IEnumerator FadeOutInBGM(AudioClip newClip, float fadeDuration = 0.5f)
    {
        float startVolume = bgmSource.volume;
        float timer = 0f;

        // Fade Out
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        bgmSource.volume = 0f;
        bgmSource.clip = newClip;
        bgmSource.Play();

        // Fade In
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }

        bgmSource.volume = startVolume;
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
        PlayBGMs(shopBGM, 0.3f); // 상점 입장할 때 빠른 페이드 (0.3초)
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
