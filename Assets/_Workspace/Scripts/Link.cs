using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    [SerializeField] private float activationTime = 0.45f;
    [SerializeField] private float activationDelay = 0.25f;
    [SerializeField] private iTween.EaseType activationEase = iTween.EaseType.easeInExpo;

    public void ActivateLink()
    {
        iTween.ScaleFrom(this.gameObject, iTween.Hash(
            "scale", Vector3.zero,
            "time", activationTime,
            "delay", activationDelay,
            "easetype", activationEase
            ));
    }
}
