using UnityEngine;
using MainMode.Items;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Filtre : ConsumablesItem
    {
        [SerializeField] private int _filtrationTime;

        public int FiltrationTime => _filtrationTime;

        public override string Name => "Фильтр";


        public void Spawn(Vector2 position)
        {
            transform.position = position;
            ShowItem();
        }

        protected override void UseConsumable()
        {
            if (user.TryGetComponent(out OxyGenSet oxyGen))
            {
                oxyGen.ChangeFitre(this);
            }
        }
    }
}