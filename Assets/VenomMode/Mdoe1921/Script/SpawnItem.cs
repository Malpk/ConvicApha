using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnItem : MonoBehaviour
{
    [Header("Destroy Mode")]
    [SerializeField] protected bool destroyMode = false;
    [SerializeField] protected float durationWork = 1;

    public abstract bool IsShow { get; }

    public abstract void SetMode(bool mode);
    public virtual void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public abstract void OffItem();
    public void SetTimeDelete(float timeDestroy)
    {
        this.durationWork = timeDestroy > 0 ? timeDestroy : 0;
    }

}