using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainMode.Items;

namespace MainMode.Mode1921
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Filtre : ConsumablesItem
    {
        [SerializeField] private int _filtrationTime;

        public int FiltrationTime => _filtrationTime;

        public void Spawn(Vector2 position)
        {
            transform.position = position;
            ShowItem();
        }
        public override void Use()
        {
            if (user.TryGetComponent<OxyGenSet>(out OxyGenSet oxyGen))
            {
                oxyGen.ChangeFitre(this);
            }
        }
    }
}