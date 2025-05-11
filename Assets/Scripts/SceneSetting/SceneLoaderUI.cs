using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoaderUI : Singleton<SceneLoaderUI>
{
    [Header("�ε� UI ������Ʈ")]
    [SerializeField] private GameObject loadingUI;

    [Header("�ε� �����̴�")]
    [SerializeField] private Slider loadingBar;

    protected override void Awake()
    {
        base.Awake(); // Singleton �ʱ�ȭ
    }

    public void LoadSceneWithLoadingScreen(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingUI.SetActive(true); // �ε� UI Ȱ��ȭ

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // 0~1 ����
            if (loadingBar != null)
            {
                loadingBar.value = progress;
            }

            yield return null;
        }

        // ���� 100%�� ä���
        if (loadingBar != null)
            loadingBar.value = 1f;

        yield return new WaitForSeconds(0.5f); // ����� ���

        operation.allowSceneActivation = true;

        // �� �ε� �� ���� �����ӿ� ��Ȱ��ȭ
        yield return null;

        loadingUI.SetActive(false);
    }
}