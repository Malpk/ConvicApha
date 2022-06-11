using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ReturnPoint : MonoBehaviour
{

    private SpriteRenderer _sprite;
    public Vector3 Position => transform.position;
    public bool State { get; private set; }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }
    public void ActiveMode(Vector3 point)
    {
        SetMode(true);
        transform.position = point;
    }

    public void Deactive(CristalMan player)
    {
        SetMode(false, player);
    }

    private void SetMode(bool mode,CristalMan player = null)
    {
        State = mode;
        _sprite.enabled = mode;
        if (player != null)
        {
            transform.parent = player.transform.parent;
            transform.localPosition = Vector3.zero;
        }
        else
        {
            transform.parent = null;
        }
    }
}
