using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 정적 클래스 : 어디서든 접근해서 서비스에 등록 / 꺼내기 가능
public static class ServiceLocator
{
    //                      키 : 인터페이스 타입, 벨류 : 객체
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Resister<T>(T service)
    {
        // 동일한 키 값이 들어 올 경우 값이 덮어짐

        services[typeof(T)] = service;
    }

    public static T Get<T>()
    {
        // 반환되는 벨류값(객체)가 (T)로 형변환 되었으니 인터페이스 타입의 오브젝트가 반환됨
        // 
        // -> 인터페이스 함수 호출 가능 

        return (T) services[typeof(T)];
    }
}
