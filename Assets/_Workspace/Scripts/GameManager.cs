using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Turn
{
    Player,
    Enemy
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<EnemyController> enemies;
    private PlayerController player;
    public Turn CurrentTurn { get; private set; } = Turn.Player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        player = FindObjectOfType<PlayerController>();
        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());
    }

    public void UpdateTurn()
    {
        if (CurrentTurn == Turn.Player)
        {
            if (player.IsTurnComplete)
            {
                // Switch to enemy turn
                CurrentTurn = Turn.Enemy;
                player.IsTurnComplete = false;

                foreach (var enemy in enemies)
                {
                    enemy.PlayEnemyTurn();
                }
            }
        }
        else if (CurrentTurn == Turn.Enemy)
        {
            if (IsEnemyTurnComplete())
            {
                // Switch to player turn
                CurrentTurn = Turn.Player;

                foreach (var enemy in enemies)
                {
                    enemy.IsTurnComplete = false;
                }
            }
        }
    }

    private bool IsEnemyTurnComplete()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.IsTurnComplete)
            {
                return false;
            }
        }
        return true;
    }
}
