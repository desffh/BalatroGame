using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawnPoint : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;

    private void Awake()
    {
        spawnPoint = GetComponent<Transform>();
    }



}
