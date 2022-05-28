using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Animator))]
    public abstract class Device : KI,IMode
    {
        [Header("DestroyMode")]
        [SerializeField] protected bool destroyMode = true;
        [SerializeField] protected float timeDestroy = 1;

        protected bool isMode = true;

        protected Animator animator;
        public abstract TrapType DeviceType { get; }

        protected void Awake()
        {
            animator = GetComponent<Animator>();
            Intilizate();
        }

        protected virtual void Intilizate()
        {
            
        }
        public virtual void TurnOff()
        {
            isMode = false;
            Invoke("DownAnimation", timeDestroy);
        }
        protected void DestroyPerfab()
        {
            Destroy(gameObject);
        }
        protected void DownAnimation()
        {
            animator.SetTrigger("Down");
        }
    }
}