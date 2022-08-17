using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Death Related")]
    [SerializeField] private float deathForwardImpulse = 2f;
    [SerializeField] private float deathUpImpulse = 4f;
    [SerializeField] private float deathRotationForce = 10f;

    public bool IsAlive { get; private set; } = true;

    public void SetIsAlive(bool value)
    {
        IsAlive = value;
    }

    public void Die(float forceMultiplier)
    {
        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        if (myRigidbody == null) { return; }

        myRigidbody.isKinematic = false;

        Vector3 deathForce = (transform.forward * deathForwardImpulse * forceMultiplier) +
            new Vector3(0f, deathUpImpulse * forceMultiplier, 0f);

        myRigidbody.AddForce(deathForce, ForceMode.Impulse);
        myRigidbody.AddForce(transform.right * deathRotationForce * forceMultiplier);

        SetIsAlive(false);
    }
}
