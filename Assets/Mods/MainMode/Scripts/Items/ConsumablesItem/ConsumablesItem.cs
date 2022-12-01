using UnityEngine;

namespace MainMode.Items
{
    public abstract class ConsumablesItem : Item
    {
        [Min(1)]
        [SerializeField] private int _countUse = 1;

        public int Count { get; private set; } = 1;

        public override bool IsUse => Count > 0;

        public override bool Use()
        {
            if (Count > 0)
            {
                Count--;
                UseConsumable();
                return true;
            }
            return false;
        }
        public override void ResetState()
        {
            Count = _countUse;
        }
        protected abstract void UseConsumable();
    }
}
