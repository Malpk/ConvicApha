using UnityEngine;

public class DeviceAttackSet : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _damage;
    [SerializeField] private DamageInfo _attackInfo;

    public void SetAttack(Player player)
    {
        if (player)
        {
            player.TakeDamage(_damage, _attackInfo);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            SetAttack(player);
        }
    }
}
