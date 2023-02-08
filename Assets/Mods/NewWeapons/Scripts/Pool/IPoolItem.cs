using UnityEngine;

public interface IPoolItem
{
    public event System.Action<IPoolItem> OnDelete;
    public GameObject PoolItem { get; }
}
