using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : TurnController
{
    [Header("Player Selection")]
    [SerializeField] private float selectedYPosition = 0.27f;
    [SerializeField] private float selectionMoveTime = 0.4f;
    [SerializeField] private float selectionMoveDelay = 0.1f;
    [SerializeField] private iTween.EaseType selectionMoveEase = iTween.EaseType.easeInOutElastic;

    [Header("Player Movement")]
    [SerializeField] private float moveTime = 0.5f;
    [SerializeField] private iTween.EaseType moveEase = iTween.EaseType.easeOutQuint;

    [Header("Player Rotation")]
    [SerializeField] private float rotateTime = 0.35f;
    [SerializeField] private float rotateDelay = 0.2f;
    [SerializeField] private iTween.EaseType rotateEase = iTween.EaseType.easeInOutExpo;

    public bool IsMoving { get; private set; } = false;
    public bool IsSelected { get; private set; } = false;
    public Spot PlayerSpot { get; private set; } = null;

    public bool ShouldTurnToDestination { get; private set; } = true;

    public Vector3 Destination { get; private set; }

    private void Start()
    {
        UpdatePlayerSpot();
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurrentTurn != Turn.Player) { return; }

        IsSelected = !IsSelected;

        float targetYPosition = IsSelected ? selectedYPosition : 0f;
        Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);

        iTween.MoveTo(this.gameObject, iTween.Hash(
            "position", targetPosition,
            "time", selectionMoveTime,
            "delay", selectionMoveDelay,
            "easetype", selectionMoveEase
            ));
    }

    public void CheckMoveTo(Vector3 destination)
    {
        if (GameManager.Instance.CurrentTurn != Turn.Player) { return; }

        Spot spotAtDestination = BoardManager.Instance.GetSpotAtPosition(destination);
        if (spotAtDestination == null) { return; }

        // If current player spot is linked to destination
        if (PlayerSpot.IsSpotLinked(spotAtDestination))
        {
            MoveTo(destination);
        }
    }

    private void MoveTo(Vector3 destination)
    {
        Destination = destination;
        IsMoving = true;
        IsSelected = false;

        iTween.MoveTo(this.gameObject, iTween.Hash(
            "position", Destination,
            "time", moveTime,
            "delay", 0f,
            "easetype", moveEase
            ));

        if (ShouldTurnToDestination)
        {
            FaceDestination();
        }

        Invoke(nameof(OnStopMoving), moveTime > rotateTime ? moveTime : rotateTime);
    }

    private void OnStopMoving()
    {
        iTween.Stop(gameObject);
        transform.position = Destination;
        IsMoving = false;

        UpdatePlayerSpot();
        FinishTurn();
    }

    private void FaceDestination()
    {
        Vector3 startToDestination = Destination - PlayerSpot.transform.position;
        Quaternion angleToTarget = Quaternion.LookRotation(startToDestination, Vector3.down);
        Vector3 targetRotation = new Vector3(0f, angleToTarget.eulerAngles.y, 0f);

        iTween.RotateTo(this.gameObject, iTween.Hash(
            "rotation", targetRotation,
            "time", rotateTime,
            "delay", rotateDelay,
            "easeType", rotateEase
            ));
    }

    public void UpdatePlayerSpot()
    {
        PlayerSpot = BoardManager.Instance.GetSpotAtPosition(transform.position);
    }
}
