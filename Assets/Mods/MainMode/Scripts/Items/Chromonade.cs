using UnityEngine;

namespace MainMode.Items
{
    public class Chromonade : ConsumablesItem
    {
        [Min(1)]
        [SerializeField] private int _healValue = 1;

        public override string Name => "Хромонаде";

        protected override void OnEnable()
        {
            UseAction += UseChromonade;
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            UseAction -= UseChromonade;
            base.OnDisable();
        }

        private void UseChromonade()
        {
            user.Heal(_healValue);
        }
    }
}