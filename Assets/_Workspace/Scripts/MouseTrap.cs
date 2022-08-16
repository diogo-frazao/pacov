using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrap : MonoBehaviour
{
    private Rigidbody myRigidbody = null;

    [Header("Trap Activation")]
    [SerializeField] private float activationTime = 0.2f;
    [SerializeField] private float activationDelay = 0f;
    [SerializeField] private iTween.EaseType activationEase = iTween.EaseType.spring;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public bool IsActivated { get; private set; } = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsActivated)
        {
            ActivateMouseTrap();

            // Call on trap activated event
            Actions.OnTrapActivated();

            IsActivated = false;
        }
    }

    private void ActivateMouseTrap()
    {
        GameObject trapSpring = transform.Find("Spring").gameObject;
        iTween.RotateTo(trapSpring, iTween.Hash(
            "z", 172f,
            "time", activationTime,
            "delay", activationDelay,
            "easetype", activationEase,
            "islocal", true
            ));
    }

    public void DeactivateMouseTrap(float explosionForce, float explosionRotationForce)
    {
        GetComponent<BoxCollider>().isTrigger = false;
        myRigidbody.isKinematic = false;

        myRigidbody.AddForce(GetRandomForceDirection() * explosionForce, ForceMode.Impulse);
        myRigidbody.AddTorque(Vector3.forward * explosionRotationForce);

        IsActivated = false;
    }

    private Vector3 GetRandomForceDirection()
    {
        int randomMultiplier = Random.Range(-1, 2);
        while (randomMultiplier == 0)
        {
            randomMultiplier = Random.Range(-1, 2);
        }
        return new Vector3(0.4f * randomMultiplier, 1f, Random.Range(-0.4f, 0.4f));
    }
}
