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
    
    public List<EnemyController> enemies { get; private set; }
    public PlayerController Player { get; private set; }
    public Turn CurrentTurn { get; private set; } = Turn.Player;

    public bool IsGameOver { get; private set; } = false;

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

        Player = FindObjectOfType<PlayerController>();
        enemies = new List<EnemyController>(FindObjectsOfType<EnemyController>());
    }

    public void UpdateTurn()
    {
        if (CurrentTurn == Turn.Player)
        {
            if (Player.IsTurnComplete)
            {
                // Switch to enemy turn
                CurrentTurn = Turn.Enemy;
                Player.IsTurnComplete = false;

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

    public void SetIsGameMover(bool value)
    {
        IsGameOver = value;
    }
}
