using UnityEngine;

namespace MainMode
{
    public class DaimonWall : MonoBehaviour, IMode
    {
        [SerializeField] private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        public void TurnOff()
        {
            Debug.Log("s");
            _animator.SetTrigger("Deactivate");
        }
    }
}
