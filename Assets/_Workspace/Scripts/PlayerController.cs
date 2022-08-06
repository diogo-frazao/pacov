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

    public bool IsSelected { get; private set; } = false;

    public override void CheckMoveTo(Vector3 destination)
    {
        if (GameManager.Instance.CurrentTurn != Turn.Player) { return; }
        base.CheckMoveTo(destination);
    }

    protected override void MoveTo(Vector3 destination)
    {
        IsSelected = false;
        base.MoveTo(destination);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurrentTurn != Turn.Player) { return; }

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
    }
}
