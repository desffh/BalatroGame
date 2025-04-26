using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance; // �̱��� �ν��Ͻ��� �����ϴ� ����

    public static T Instance
    {
        get
        {
            if (instance == null) // instance�� ��� �ִٸ�
            {
                instance = FindObjectOfType<T>(); // ���� ������ T Ÿ�� ������Ʈ�� ã��

                if (instance == null) // ���� ������ ���� ����
                {
                    GameObject singletonObj = new GameObject(typeof(T).Name);
                    instance = singletonObj.AddComponent<T>();
                    DontDestroyOnLoad(singletonObj);
                }
            }
            return instance; // instance ��ȯ
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;  // ���⼭ �ڵ� ���
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;  //  �ڵ� ����
            instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeReferences();
    }

    //  �ڽ� Ŭ������ override �ϴ� �κ�
    protected virtual void InitializeReferences()
    {
        // �⺻�� �ƹ��͵� �� ��
    }

    // �߰�: �̱��� ������ �����ϴ� �Լ�
    public static void DestroySelf()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}