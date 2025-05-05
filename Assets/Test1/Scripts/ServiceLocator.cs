using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� Ŭ���� : ��𼭵� �����ؼ� ���񽺿� ��� / ������ ����
public static class ServiceLocator
{
    //                      Ű : �������̽� Ÿ��, ���� : ��ü
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Resister<T>(T service)
    {
        // ������ Ű ���� ��� �� ��� ���� ������

        services[typeof(T)] = service;
    }

    public static T Get<T>()
    {
        // ��ȯ�Ǵ� ������(��ü)�� (T)�� ����ȯ �Ǿ����� �������̽� Ÿ���� ������Ʈ�� ��ȯ��
        // 
        // -> �������̽� �Լ� ȣ�� ���� 

        return (T) services[typeof(T)];
    }
}
