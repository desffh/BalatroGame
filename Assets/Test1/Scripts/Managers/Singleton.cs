using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance; // 싱글톤 인스턴스를 저장하는 변수

    public static T Instance
    {
        get
        {
            if (instance == null) // instance가 비어 있다면
            {
                instance = FindObjectOfType<T>(); // 현재 씬에서 T 타입 오브젝트를 찾음

                if (instance == null) // 씬에 없으면 새로 생성
                {
                    GameObject singletonObj = new GameObject(typeof(T).Name);
                    instance = singletonObj.AddComponent<T>();
                    DontDestroyOnLoad(singletonObj);
                }
            }
            return instance; // instance 반환
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;  // 여기서 자동 등록
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
            SceneManager.sceneLoaded -= OnSceneLoaded;  //  자동 해제
            instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeReferences();
    }

    //  자식 클래스가 override 하는 부분
    protected virtual void InitializeReferences()
    {
        // 기본은 아무것도 안 함
    }

    // 추가: 싱글톤 스스로 제거하는 함수
    public static void DestroySelf()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
    }
}