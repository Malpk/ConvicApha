using System;
using UnityEngine;

namespace Mods.CrystalMode.Scripts
{
    public class Dozazdatel : MonoBehaviour, IDamage
    {
        [SerializeField] private Transform target;
        [SerializeField] private int health;
        [SerializeField] private BaseState startState;
        [SerializeField] private BaseState attackState;
        [SerializeField] private BaseState attachedToPlayerState;
        private BaseState currentState;


        private void OnTriggerEnter2D(Collider2D col)
        {
            
        }

        private void Update()
        {
            currentState.Update();
        }

        public void Explosion()
        {
            Destroy(gameObject);
        }

        public void TakeDamage(int damage, DamageInfo type)
        {
            health -= damage;
            if (health <= 0)
            {
                Explosion();
            }
        }
    }
}
