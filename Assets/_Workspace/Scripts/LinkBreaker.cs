using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBreaker : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0f, 0f, 0.5f);
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
