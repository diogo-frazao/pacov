using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Stationary,
    Patrol
}


[RequireComponent(typeof(EnemyDetector))]
public class EnemyController : MovementController
{
    private EnemyDetector myEnemyDetector;

    [Header("Enemy Specific")]
    [SerializeField] private Vector3 directionToMove = new Vector3(0f, 0f, BoardManager.SpotsSpacing);
    [SerializeField] private EnemyType myEnemyType;

    private void Awake()
    {
        myEnemyDetector = GetComponent<EnemyDetector>();
    }

    public void PlayEnemyTurn()
    {
        myEnemyDetector.TryDetectPlayer();
        EnemyActionBasedOnType();
    }

    private void EnemyActionBasedOnType()
    {
        switch (myEnemyType)
        {
            case EnemyType.Stationary:
                StartCoroutine(StayStillRoutine());
                break;
            case EnemyType.Patrol:
                StartCoroutine(PatrolRoutine());
                break;
        }
    }

    private IEnumerator PatrolRoutine()
    {
        Vector3 startPosition = new Vector3(CurrentSpot.SpotCoordinate.x, 0f, CurrentSpot.SpotCoordinate.y);

        // One node away
        Vector3 nextNodePosition = startPosition + transform.TransformVector(directionToMove);
        // Two nodes away
        Vector3 nextNextNodePosition = startPosition + transform.TransformVector(directionToMove * 2);

        MoveTo(nextNodePosition);
        while (IsMoving) { yield return null; }

        Spot nextSpot = BoardManager.Instance.GetSpotAtPosition(nextNodePosition);
        Spot nextNextSpot = BoardManager.Instance.GetSpotAtPosition(nextNextNodePosition);

        // If I've come to an edge, turn
        if (nextNextSpot == null || !nextSpot.IsSpotLinked(nextNextSpot))
        {
            Destination = startPosition;
            FaceDestination();
            yield return new WaitForSeconds(rotateTime);
        }
        FinishTurn();
    }

    private IEnumerator StayStillRoutine()
    {
        yield return new WaitForSeconds(1f);
        FinishTurn();
    }
}