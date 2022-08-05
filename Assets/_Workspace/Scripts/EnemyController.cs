using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyDetector))]
public class EnemyController : TurnController
{
    private EnemyDetector myEnemyDetector;

    private void Awake()
    {
        myEnemyDetector = GetComponent<EnemyDetector>();
    }

    public void PlayEnemyTurn()
    {
        myEnemyDetector.TryDetectPlayer();
        StartCoroutine(MoveEnemy());
    }

    private IEnumerator MoveEnemy()
    {
        yield return new WaitForSeconds(5f);
        FinishTurn();
    }
}
