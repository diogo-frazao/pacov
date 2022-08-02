using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    [Header("Spot Activation")]
    [SerializeField] private float activationTime = 0.4f;
    [SerializeField] private float activationDelay = 0.4f;
    [SerializeField] private iTween.EaseType activationEase = iTween.EaseType.easeOutQuint;
    [SerializeField] private bool isStartingNode = false;
    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private LayerMask linkBreakerLayer;

    [SerializeField] private float timeBetweenNodesActivation = 0.5f; 

    public Vector2 SpotCoordinate { get; private set; }

    private List<Spot> neighborSpots = new List<Spot>();
    private List<Spot> linkedSpots = new List<Spot>();

    // Avoid already activated node being reactivated
    public bool IsActive { get; private set; } = false;

    private void Awake()
    {
        transform.localScale = Vector3.zero;

        SpotCoordinate = new Vector2(Mathf.RoundToInt(transform.position.x), 
            Mathf.RoundToInt(transform.position.z));
    }

    private void Start()
    {
        if (isStartingNode)
        {
            ActivateSpot();
            ShowNeighbors();
        }
        PopulateNeighborSpots();
    }

    private void ShowNeighbors()
    {
        StartCoroutine(ShowNeighborsRoutine());
    }

    private IEnumerator ShowNeighborsRoutine()
    {
        yield return new WaitForSeconds(timeBetweenNodesActivation);

        foreach (Spot neighbor in neighborSpots)
        {
            if (!linkedSpots.Contains(neighbor))
            {
                LinkBreaker linkBreaker = CheckLinkBreaker(this, neighbor);
                if (linkBreaker == null)
                {
                    DrawLinkBetween(this, neighbor);
                    if (neighbor.IsActive) { continue; }
                    neighbor.ActivateSpot();
                    neighbor.ShowNeighbors();
                }
            }
        }
    }

    private void DrawLinkBetween(Spot startSpot, Spot targetSpot)
    {
        if (!linkedSpots.Contains(targetSpot))
        {
            linkedSpots.Add(targetSpot);
        }

        Vector3 startToTargetSpot = targetSpot.transform.position - startSpot.transform.position;

        // Place link a bit in front of startingSpot (avoid seeing edges inside the circle)
        Vector3 directionToMoveLink = startToTargetSpot.normalized * 0.06f;
        Vector3 linkSpawnPosition = startSpot.transform.position + directionToMoveLink;

        // Create link
        GameObject linkCreated = Instantiate(linkPrefab, linkSpawnPosition, Quaternion.identity);

        // Rotate link to targetSpot
        Quaternion linkRotation = Quaternion.LookRotation(startToTargetSpot, Vector3.up);
        linkCreated.transform.rotation = linkRotation;

        linkCreated.transform.parent = BoardManager.Instance.GetLinksParent();

        linkCreated.GetComponent<Link>()?.ActivateLink();
    }

    private void ActivateSpot()
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash(
            "scale", Vector3.one,
            "time", activationTime,
            "delay", activationDelay,
            "easetype", activationEase
            ));
        IsActive = true;
    }

    private LinkBreaker CheckLinkBreaker(Spot startSpot, Spot targetSpot)
    {
        Vector3 direction = targetSpot.transform.position - transform.position;
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, direction, out raycastHit,
            BoardManager.SpotsSpacing + 0.1f, linkBreakerLayer))
        {
            return raycastHit.collider.GetComponent<LinkBreaker>();
        }
        return null;
    }

    private void PopulateNeighborSpots()
    {
        List<Spot> allSpots = BoardManager.Instance.GetAllSpotsInScene();

        foreach (var direction in BoardManager.Instance.BoardDirections)
        {
            Spot neighborSpot = allSpots.Find(n => n.SpotCoordinate == this.SpotCoordinate + direction);

            if (neighborSpot != null && !neighborSpots.Contains(neighborSpot))
            {
                neighborSpots.Add(neighborSpot);
            }
        }
    }
}
