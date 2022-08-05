using UnityEngine;

namespace MainMode
{
    public class DaimonWall : MonoBehaviour,IDamage
    {
        [Min(1)]
        [SerializeField] private int _healhtWall = 1;
        [SerializeField] private Animator _animator;

        private int _curretHealht;

        private void Awake()
        {
            _curretHealht = _healhtWall;
            _animator = GetComponent<Animator>();
        }
        public void Activate()
        {
            _curretHealht = _healhtWall;
            _animator.SetBool("Mode", false);
        }

        public void Dead()
        {
            Activate();
        }

        public void TakeDamage(int damage, DamageInfo type)
        {
            _curretHealht -= damage;
            if (_curretHealht <= 0)
                Dead();

        }
    }
}
