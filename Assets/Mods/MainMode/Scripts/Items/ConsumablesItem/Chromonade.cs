using UnityEngine;

namespace MainMode.Items
{
    public class Chromonade : ConsumablesItem
    {
        [Min(1)]
        [SerializeField] private int _healValue = 1;

        public override string Name => "Хромонаде";

        protected override void UseConsumable()
        {
            user.Heal(_healValue);
        }
    }
}