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
}
