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

    private AudioClip previousClip;    // ���� ����Ǵ� BGM
    private float previousTime;        // ���� ����Ǵ� �ð�
    private bool hasPrevious = false;  // ���� �������� ����

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

    // �Ϲ� ���� BGM ���
    public void PlayGameBGM()
    {
        PlayBGM(gameBGM);
    }

    // BGM ���
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

        fadeCoroutine = StartCoroutine(FadeOutInBGM(clip, fadeDuration)); // ����!
    }





    public void ResumePreviousBGM()
    {
        if (hasPrevious && previousClip != null)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeOutInBGM(previousClip, 0.5f)); // ���͵� �ε巴��
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
        PlayBGMs(shopBGM, 0.3f); // ���� ������ �� ���� ���̵� (0.3��)
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
