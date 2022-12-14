using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; private set; }

    public static float SpotsSpacing { get; private set; } = 2f;

    public readonly Vector2[] BoardDirections =
    {
        new Vector2(SpotsSpacing, 0f),
        new Vector2(-SpotsSpacing, 0f),
        new Vector2(0f, SpotsSpacing),
        new Vector2(0f, -SpotsSpacing)
    };

    private List<Spot> allSpotsInScene;

    private Transform linksParent = null;
    private PlayerController player;

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

        allSpotsInScene = new List<Spot>(FindObjectsOfType<Spot>());
    }

    public List<Spot> GetAllSpotsInScene()
    {
        return allSpotsInScene;
    }

    public Transform GetLinksParent()
    {
        linksParent = linksParent == null ? GameObject.Find("LinksParent").transform : linksParent;
        return linksParent;
    }

    public Spot GetSpotAtPosition(Vector3 positionToCheck)
    {
        return allSpotsInScene.Find(n => n.transform.position == positionToCheck);
    }

    public Spot GetPlayerSpot()
    {
        player = player == null ? FindObjectOfType<PlayerController>() : player;
        return player.CurrentSpot;
    }

    public EnemyController GetEnemyAtLocation(Vector3 location)
    {
        Spot spotAtLocation = GetSpotAtPosition(location);
        if (spotAtLocation == null) { return null; }

        foreach (var enemy in GameManager.Instance.enemies)
        {
            if (enemy.CurrentSpot == spotAtLocation)
            {
                print(enemy.gameObject.name + " is in the spot player will move towards");
                return enemy;
            }
        }
        return null;
    }
}
