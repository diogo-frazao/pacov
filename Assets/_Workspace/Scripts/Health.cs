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

    public void Die(Vector3 deathDirection, Vector3 rotationDirection, float forceMultiplier)
    {
        // Ignore collisions with objects (avoids getting inside other rigid bodies)
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");

        StartCoroutine(DieRoutine(deathDirection, rotationDirection, forceMultiplier));
    }

    private IEnumerator DieRoutine(Vector3 deathDirection, Vector3 rotationDirection, float forceMultiplier)
    {
        yield return new WaitForSeconds(0.2f);

        Rigidbody myRigidbody = GetComponent<Rigidbody>();
        if (myRigidbody != null)
        {
            myRigidbody.isKinematic = false;

            Vector3 deathForce = (deathDirection * deathForwardImpulse * forceMultiplier) +
                new Vector3(0f, deathUpImpulse * forceMultiplier, 0f);

            myRigidbody.AddForce(deathForce, ForceMode.Impulse);
            myRigidbody.AddRelativeTorque(rotationDirection * deathRotationForce * forceMultiplier);

            SetIsAlive(false);
        }
    }
}
