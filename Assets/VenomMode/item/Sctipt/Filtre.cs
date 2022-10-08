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


        private void OnEnable()
        {
            UseAction += Actvate;
        }
        private void OnDisable()
        {
            UseAction -= Actvate;
        }
        public void Spawn(Vector2 position)
        {
            transform.position = position;
            ShowItem();
        }
        private void Actvate()
        {
            if (user.TryGetComponent<OxyGenSet>(out OxyGenSet oxyGen))
            {
                oxyGen.ChangeFitre(this);
            }
        }
    }
}