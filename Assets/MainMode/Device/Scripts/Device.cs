using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.GameMechanics;

namespace MainMode
{
    [RequireComponent(typeof(Animator))]
    public abstract class Device : KI, IMode
    {
        [Header("DestroyMode")]
        [SerializeField] protected bool destroyMode = true;
        [SerializeField] protected float timeDestroy = 1;
        private Vector3Int _cellPos;
        public Vector3Int CellPos { get => _cellPos; set => _cellPos = value; }

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
            MoveUpAnimation();
        }
        public virtual void TurnOff()
        {
            isMode = false;
            Invoke("DownAnimation", timeDestroy);
        }
        protected void DestroyPerfab()
        {         
            PlayGround.Instance.DeleteDeviceOnCell(_cellPos);
            Destroy(gameObject);
        }
        protected void DownAnimation()
        {
            animator.SetTrigger("Down");
        }

        protected void MoveUpAnimation()
        {
            animator.SetTrigger("Up");
        }
    }
}