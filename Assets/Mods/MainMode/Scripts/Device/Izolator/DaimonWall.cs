using UnityEngine;

namespace MainMode
{
    public class DaimonWall : JetPoint, IDamage
    {
        [Min(1)]
        [SerializeField] private int _healhtWall = 1;

        private int _curretHealht;

        private void Awake()
        {
            _curretHealht = _healhtWall;
        }
        public void Activate()
        {
            _curretHealht = _healhtWall;
        }

        public void Explosion(AttackType attack = AttackType.None)
        {
            Activate();
        }

        public void TakeDamage(int damage, DamageInfo type)
        {
            _curretHealht -= damage;
            if (_curretHealht <= 0)
                Explosion();
        }

        public override void SetAttack(DamageInfo info)
        {
        }
    }
}
