using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardComponent : MonoBehaviour
{
    public abstract void OnCollider();

    public abstract void OffCollider();

    public abstract void OnMouse();
}
