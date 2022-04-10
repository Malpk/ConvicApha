using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Device : MonoBehaviour
{
    [SerializeField]
    protected float _timeOfAction;

    protected abstract void ActivateDeviceOnPlayer(PlayerController playerController);

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.attachedRigidbody?.GetComponent<PlayerController>();
        if (playerController)
        {
            ActivateDeviceOnPlayer(playerController);
        }
    }
}
