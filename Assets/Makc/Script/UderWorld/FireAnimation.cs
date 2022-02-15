using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Underworld
{
    public class FireAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        public void OnFireDestroy()
        {
            Destroy(gameObject);
        }
        private void OnIdleMode()
        {
            if (_animator != null)
                _animator.SetBool("IdleMode", true);;
        }
    }
}