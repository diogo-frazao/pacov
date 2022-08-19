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
    public EnemyDetector MyEnemyDetector { get; private set; }

    [Header("Enemy Specific")]
    [SerializeField] private Vector3 directionToMove = new Vector3(0f, 0f, BoardManager.SpotsSpacing);
    [SerializeField] private EnemyType myEnemyType;

    private Health myHealthComponent;

    private void Awake()
    {
        MyEnemyDetector = GetComponent<EnemyDetector>();
        myHealthComponent = GetComponent<Health>();
    }

    public void PlayEnemyTurn()
    {
        // If enemy is dead, don't do anything in his turn
        if (myHealthComponent.IsAlive == false)
        {
            FinishTurn();
            return;
        }

        MyEnemyDetector.TryDetectPlayer();
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

        // After moving to new spot, check if player should be killed
        CheckKillPlayer();

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
        CheckKillPlayer();
        FinishTurn();
    }

    private void CheckKillPlayer()
    {
        // After moving and detecting player, check for player kill
        if (MyEnemyDetector.WasPlayerFound)
        {
            if (CurrentSpot == BoardManager.Instance.GetPlayerSpot())
            {
                CallOnPlayerKilled();
            }
            else
            {
                MoveTo(GameManager.Instance.Player.transform.position);
                Invoke(nameof(CallOnPlayerKilled), moveTime);
            }
        }
    }

    private void CallOnPlayerKilled()
    {
        Actions.OnPlayerKilled(transform.forward);
    }

    /** Enemy Death */

    public void ScareEnemy(Vector3 directionToThrowEnemy)
    {
        GetComponent<CapsuleCollider>().isTrigger = false;
        
        myHealthComponent.Die(directionToThrowEnemy, directionToThrowEnemy, 1f);   
    }


}
