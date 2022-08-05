using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public bool IsTurnComplete { get; set; } = false;

    public void FinishTurn()
    {
        IsTurnComplete = true;
        GameManager.Instance.UpdateTurn();
    }
}
