using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoroutineCache
{
    // 참조타입이라 new로 생성 (C++)
    static Dictionary<float, WaitForSeconds> dictionary = new Dictionary<float, WaitForSeconds>();

    // 정적 함수 생성하여 다른 클래스에서 사용

    // time = 키 값
    // WaitForSeconds 타입을 반환
    public static WaitForSeconds WaitForSeconds(float time)
    {
        // 내부에는 정적인 것만 가능 (정적변수)

        // 딕셔너리 키값에 해당 시간만큼의 WaitForSeconds 생성

        // TryGetValue 사용하여 키 값이 없다면 추가한뒤 반환
        // 키 값이 있다면 그냥 반환

        // 참조변수
        WaitForSeconds waitForSeconds;

        if (dictionary.TryGetValue(time, out waitForSeconds) == false)
        {
            dictionary.Add(time, new WaitForSeconds(time));

            // 추가 한 뒤에 참조해야한다 -> 추가 하기 전까지는 키벨류가 null 상태

            waitForSeconds = dictionary[time];
        }

        return waitForSeconds;
    }
}