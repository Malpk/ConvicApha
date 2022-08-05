using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public abstract class SpawnItem : MonoBehaviour
    {
        [Header("Destroy Mode")]
        [SerializeField] protected bool destroyMode = false;
        [SerializeField] protected float timeDestroy = 1;

        public abstract bool IsShow { get; }

        public abstract void SetMode(bool mode);
        public virtual void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public abstract void Deactivate();
        public void SetTimeDelete(float timeDestroy)
        {
            this.timeDestroy = timeDestroy > 0 ? timeDestroy : 0;
        }

    }
}