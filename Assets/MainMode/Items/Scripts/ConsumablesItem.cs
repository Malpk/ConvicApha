using UnityEngine;

namespace MainMode.Items
{
    public abstract class ConsumablesItem : Item
    {
        [SerializeField] protected float resistDuration;
        [SerializeField] protected DamageInfo attackResist;
    }
}
