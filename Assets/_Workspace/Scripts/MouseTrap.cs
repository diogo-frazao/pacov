using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrap : MonoBehaviour
{
    public bool IsActivated { get; private set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsActivated)
        {
            Debug.LogWarning("Player dead");
        }
    }

    public void DeactivateMouseTrap()
    {
        IsActivated = false;
    }
}
