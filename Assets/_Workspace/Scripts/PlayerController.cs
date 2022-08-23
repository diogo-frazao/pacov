using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementController
{
    [Header("Player Selection")]
    [SerializeField] private float selectedYPosition = 0.27f;
    [SerializeField] private float selectionMoveTime = 0.4f;
    [SerializeField] private float selectionMoveDelay = 0.1f;
    [SerializeField] private iTween.EaseType selectionMoveEase = iTween.EaseType.easeInOutElastic;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip playerSelectionSound;
    [SerializeField] private AudioClip playerPieceDownSound;
    [SerializeField] private AudioClip playerDeathHitSound;

    public bool IsSelected { get; private set; } = false;

    private Rigidbody myRigidbody = null;
    private SphereCollider mySphereCollider = null;

    // Optional component
    private Health myHealthComponent;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        mySphereCollider = GetComponent<SphereCollider>();
        myHealthComponent = GetComponent<Health>();
    }

    private void OnEnable()
    {
        Actions.OnTrapActivated += PlayerMouseTrapDeath;
        Actions.OnPlayerKilled += PlayerDeath;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (myHealthComponent.IsAlive == false)
        {
            SoundManager.Instance.PlayAudio(playerDeathHitSound, 1.5f, true);
        }
    }

    public override void CheckMoveTo(Vector3 destination)
    {
        if (myHealthComponent.IsAlive == false) { return; }
        if (GameManager.Instance.CurrentTurn != Turn.Player) { return; }
        base.CheckMoveTo(destination);
    }

    protected override void MoveTo(Vector3 destination)
    {
        IsSelected = false;

        // Check if there's an enemy in the node I'm moving towards
        EnemyController enemyAtDestination = BoardManager.Instance.GetEnemyAtLocation(destination);
        if (enemyAtDestination && enemyAtDestination.MyEnemyDetector.WasPlayerFound == false)
        {
            print(enemyAtDestination + " was scared");
            enemyAtDestination.ScareEnemy(transform.forward);
        }

        SoundManager.Instance.PlayAudio(playerPieceDownSound, 1f, false);

        base.MoveTo(destination);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurrentTurn != Turn.Player || 
            myHealthComponent.IsAlive == false) { return; }

        // Select player
        IsSelected = !IsSelected;

        float targetYPosition = IsSelected ? selectedYPosition : 0f;
        Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);

        iTween.MoveTo(this.gameObject, iTween.Hash(
            "position", targetPosition,
            "time", selectionMoveTime,
            "delay", selectionMoveDelay,
            "easetype", selectionMoveEase
            ));

        SoundManager.Instance.PlayAudio(playerSelectionSound, 1f, false);
    }

    /** Player Death */

    private void PlayerMouseTrapDeath()
    {
        // Ignore collisions with objects (avoids getting inside other rigid bodies)
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");

        mySphereCollider.isTrigger = true;

        myHealthComponent.Die(transform.forward, transform.right, 1f);
    }

    public void PlayerDeath(Vector3 directionToThrowPlayer)
    {
        GameManager.Instance.SetIsGameMover(true);

        // Ignore collisions with objects (avoids getting inside other rigid bodies)
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");

        mySphereCollider.isTrigger = true;

        myHealthComponent.Die(directionToThrowPlayer, -directionToThrowPlayer, 1.25f);
    }
}
