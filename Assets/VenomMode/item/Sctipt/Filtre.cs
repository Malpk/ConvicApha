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
            SetMode(true);
        }
        public override void Use()
        {
            if (_target.TryGetComponent<OxyGenSet>(out OxyGenSet oxyGen))
            {
                oxyGen.ChangeFitre(this);
            }
        }
    }
}