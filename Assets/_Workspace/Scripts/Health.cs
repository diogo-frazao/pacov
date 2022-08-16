using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool IsAlive { get; private set; } = true;

    public void SetIsAlive(bool value)
    {
        IsAlive = value;
    }
}
