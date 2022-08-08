using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsButton : MonoBehaviour
{
    private bool wasActivated = false;
    private List<MouseTrap> mouseTrapsInScene;

    private void Awake()
    {
        mouseTrapsInScene = new List<MouseTrap>(FindObjectsOfType<MouseTrap>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !wasActivated)
        {
            foreach (var mouseTrap in mouseTrapsInScene) { mouseTrap.DeactivateMouseTrap(); }
            // TODO change height and color to active
            wasActivated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO change height and color to inactive
        }
    }
}
