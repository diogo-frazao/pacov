using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private Vector3 directionToSearchPlayer = new Vector3(0, 0, 2);

    public bool WasPlayerFound { get; private set; }
    private Spot spotToSearchPlayerAt;

    private void Update()
    {
        TryDetectPlayer();
    }

    public void TryDetectPlayer()
    {
        Vector3 worldPositionToSearchPlayer = transform.position + transform.TransformVector(
            directionToSearchPlayer);

        spotToSearchPlayerAt = BoardManager.Instance.GetSpotAtPosition(worldPositionToSearchPlayer);

        if (spotToSearchPlayerAt == BoardManager.Instance.GetPlayerSpot())
        {
            // Avoid detecting player if searching node is not connected to mine
            Spot mySpot = BoardManager.Instance.GetSpotAtPosition(transform.position);
            if (mySpot == null || !mySpot.IsSpotLinked(spotToSearchPlayerAt)) { return; }

            WasPlayerFound = true;
        }
    }
}
