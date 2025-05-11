using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoaderUI : Singleton<SceneLoaderUI>
{
    [Header("로딩 UI 오브젝트")]
    [SerializeField] private GameObject loadingUI;

    [Header("로딩 슬라이더")]
    [SerializeField] private Slider loadingBar;

    protected override void Awake()
    {
        base.Awake(); // Singleton 초기화
    }

    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingUI.SetActive(true); // 로딩 UI 활성화

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 0~1 보정
            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }

            yield return null;
        }

        // 최종 100%로 채우기
        if (loadingBar != null)
            loadingBar.value = 1f;

        yield return new WaitForSeconds(0.5f); // 연출용 대기

        operation.allowSceneActivation = true;

        // 씬 로드 후 다음 프레임에 비활성화
        yield return null;

        loadingUI.SetActive(false);
    }
}