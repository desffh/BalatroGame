using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour, IAudioService, IAudioServicePitch
{
    private static SoundManager instance;

    public AudioSource bgmSource;

    public AudioSource sfxSource;

    public List<AudioClip> audioclips;

    private Dictionary<string, AudioClip> clipdict = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 이미 존재하면 새로 생긴 건 파괴
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        foreach (AudioClip clip in audioclips)
        {
            if(!clipdict.ContainsKey(clip.name))
            {
                clipdict.Add(clip.name, clip);
            }
        }

        // AudioManager 객체를 서비스 등록 
        ServiceLocator.Resister<IAudioService>(this);
        ServiceLocator.Resister<IAudioServicePitch>(this);
    }

    public void Start()
    {
        bgmSource.volume = 0.5f;
        sfxSource.volume = 0.5f;

        ServiceLocator.Get<IAudioService>().PlayBGM("MainTheme-Title", true);
    }

    // 인터페이스 구현-------------------------------

    public void PlaySFX(string clipname)
    {
        sfxSource.pitch = 1;

        if (clipdict.TryGetValue(clipname, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySFXPitch(string clipname)
    {
        if (clipdict.TryGetValue(clipname, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
            sfxSource.pitch += 0.04f;    
        }
    }

    public void PlayBGM(string clipname, bool loop = true)
    {
        if (clipdict.TryGetValue(clipname, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = loop;

            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }
    
    // |------------------------------
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

}
