using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Piece Movement")]
    [SerializeField] protected float moveTime = 0.5f;
    [SerializeField] protected iTween.EaseType moveEase = iTween.EaseType.easeOutQuint;

    [Header("Piece Rotation")]
    [SerializeField] protected bool shouldTurnToDestination = true;
    [SerializeField] protected float rotateTime = 0.35f;
    [SerializeField] protected float rotateDelay = 0.2f;
    [SerializeField] protected iTween.EaseType rotateEase = iTween.EaseType.easeInOutExpo;

    public bool IsMoving { get; private set; } = false;
    public Spot CurrentSpot { get; set; } = null;
    public Vector3 Destination { get; set; }
    public bool IsTurnComplete { get; set; } = false;

    protected virtual void Start()
    {
        UpdateCurrentSpot();
    }

    public void FinishTurn()
    {
        IsTurnComplete = true;
        GameManager.Instance.UpdateTurn();
    }

    public virtual void CheckMoveTo(Vector3 destination)
    {
        Spot spotAtDestination = BoardManager.Instance.GetSpotAtPosition(destination);
        if (spotAtDestination == null) { return; }

        // If current player spot is linked to destination
        if (CurrentSpot.IsSpotLinked(spotAtDestination))
        {
            MoveTo(destination);
        }
    }

    protected virtual void MoveTo(Vector3 destination)
    {
        Destination = destination;
        IsMoving = true;

        iTween.MoveTo(this.gameObject, iTween.Hash(
            "position", Destination,
            "time", moveTime,
            "delay", 0f,
            "easetype", moveEase
            ));

        if (shouldTurnToDestination)
        {
            FaceDestination();
        }

        Invoke(nameof(OnStopMoving), moveTime > rotateTime ? moveTime : rotateTime);
    }

    protected void OnStopMoving()
    {
        iTween.Stop(gameObject);
        transform.position = Destination;
        IsMoving = false;

        UpdateCurrentSpot();
        FinishTurn();
    }

    protected void FaceDestination()
    {
        Vector3 startToDestination = Destination - CurrentSpot.transform.position;
        Quaternion angleToTarget = Quaternion.LookRotation(startToDestination, Vector3.down);
        Vector3 targetRotation = new Vector3(0f, angleToTarget.eulerAngles.y, 0f);

        iTween.RotateTo(this.gameObject, iTween.Hash(
            "rotation", targetRotation,
            "time", rotateTime,
            "delay", rotateDelay,
            "easeType", rotateEase
            ));
    }

    protected void UpdateCurrentSpot()
    {
        CurrentSpot = BoardManager.Instance.GetSpotAtPosition(transform.position);
    }
}
