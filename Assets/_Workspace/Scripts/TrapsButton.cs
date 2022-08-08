using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsButton : MonoBehaviour
{
    [Header("Traps Deactivation")]
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionRotationForce = 7f;

    private bool wasActivated = false;
    private List<MouseTrap> mouseTrapsInScene;

    [Header("Button Activation")]
    [SerializeField] private float buttonDeactivatedY = 0.09f;
    [SerializeField] private float buttonActivatedY = -0.104f;
    [SerializeField] private Material buttonDeactivatedMaterial;
    [SerializeField] private Material activatedMaterial;

    private void Awake()
    {
        mouseTrapsInScene = new List<MouseTrap>(FindObjectsOfType<MouseTrap>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!wasActivated)
            {
                Invoke(nameof(CallDeactivateMouseTraps), 0.2f);
                // TODO change height and color to active
                wasActivated = true;
            }
            StartCoroutine(ActivateTrapButton(true));
        }
    }

    private void CallDeactivateMouseTraps()
    {
        foreach (var mouseTrap in mouseTrapsInScene) 
        { 
            mouseTrap.DeactivateMouseTrap(explosionForce, explosionRotationForce); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ActivateTrapButton(false));
        }
    }

    private IEnumerator ActivateTrapButton(bool value)
    {
        Transform button = transform.Find("Button");

        float targetYPosition = value ? buttonActivatedY : buttonDeactivatedY;

        iTween.MoveTo(button.gameObject, iTween.Hash(
            "y", targetYPosition,
            "islocal", true,
            "time", 0.75f,
            "delay", 0f,
            "easetype", iTween.EaseType.easeOutExpo
            ));

        yield return new WaitForSeconds(0.1f);

        Material targetMaterial = value ? activatedMaterial : buttonDeactivatedMaterial;
        button.GetComponent<Renderer>().material = targetMaterial;
    }
}
