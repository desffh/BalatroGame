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

    // �Ϲ� ���� BGM ���
    public void PlayGameBGM()
    {
        PlayBGM(gameBGM);
    }

    // BGM ���
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
        float fadeDuration = 0.05f;  // ���̵� �ð� (�� ����)
        float startVolume = 0.3f;

        // 1. ���� BGM ���̵� �ƿ�
        while (bgmSource.volume > 0)
        {
            bgmSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.volume = 0f;

        bgmSource.Play();  // �� Clip ó������ Play

        // 2. �� BGM ���̵� ��
        while (bgmSource.volume < startVolume)
        {
            bgmSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        bgmSource.volume = startVolume;  // ��Ȯ�� ���߱�
    }

    // |-------------------------------------

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // ī�忡 ����� �� ����ϴ� �Լ�
    public void PlayCardEnter()
    {
        //Debug.Log("ȿ���� ���");
        PlaySFX(cardEnter);
    }

    // ī�� ������ �� ����ϴ� �Լ�
    public void PlayCardSpawn()
    {
        PlaySFX(cardSpawn);
    }

    // ī�带 Ŭ������ �� ȣ���ϴ� �Լ�
    public void PlayCardClick()
    {
        PlaySFX(cardClick);
    }

    // ī�尡 ���� �� ȣ��Ǵ� �Լ�
    public void CheckCard()
    {
        PlaySFX(checkCard);
    }

    // ���� �������� ����
    public void OnBossStageStart()
    {
        PlayBGMs(bossBGM);
    }

    // ���� Ŭ����
    public void OnBossStageClear()
    {
        PlayGameBGM();
    }

    // ���� �������� ����
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
        if (checkCard != null)  // ī�� ���� �Ҹ� Ŭ��
        {
            sfxSource.PlayOneShot(checkCard);
        }
    }

    public void ResetSFXPitch()
    {
        sfxSource.pitch = 1.0f;
    }

}
