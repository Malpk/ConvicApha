using UnityEngine;
using MainMode.Items;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Filtre : ConsumablesItem
    {
        [SerializeField] private int _filtrationTime;

        public int FiltrationTime => _filtrationTime;

        protected override void OnEnable()
        {
            base.OnEnable();
            UseAction += Actvate;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            UseAction -= Actvate;
        }
        public void Spawn(Vector2 position)
        {
            transform.position = position;
            ShowItem();
        }
        private void Actvate()
        {
            if (user.TryGetComponent(out OxyGenSet oxyGen))
            {
                oxyGen.ChangeFitre(this);
            }
        }
    }
}