using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    [RequireComponent(typeof(Animator))]
    public abstract class Device : SpawnItem
    {

        [SerializeField] protected bool playOnStart = true;

        private bool _isMode = false;

        public override bool IsShow => _isMode;

        protected Animator animator;
        private Coroutine _destroy = null;

        public abstract TrapType DeviceType { get; }

        protected void Awake()
        {
            animator = GetComponent<Animator>();
            Show();
            Intilizate();
        }
        protected abstract void Intilizate();

        public virtual void Run()
        {
            SetMode(true);
        }


        public override void SetMode(bool mode)
        {
            if (mode)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
        protected void Activate()
        {
            SetState(true);
            if (_destroy == null)
            {
                if (destroyMode)
                    _destroy = StartCoroutine(Delete(timeDestroy));
            }
        }
        public override void Deactivate()
        {
            _destroy = null;
            _isMode = false;
            SetState(false);
            animator.SetTrigger("Drop");
        }

        private void Hide()
        {
            animator.SetBool("Show", false);
        }

        private void Show()
        {
            _isMode = true;
            animator.SetBool("Show",true );
        }
        protected abstract void SetState(bool mode);
        private IEnumerator Delete(float timeDestroy)
        {
            var progress = 0f;
            while (IsShow && progress < 1f)
            {
                progress += Time.deltaTime / timeDestroy;
                yield return null;
            }
            if(IsShow)
                Hide();
        }
    }
}