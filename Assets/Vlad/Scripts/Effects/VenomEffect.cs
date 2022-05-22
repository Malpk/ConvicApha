using System.Collections;
using UnityEngine;

namespace BaseMode
{
    public class VenomEffect : MonoBehaviour
    {
        [Min(1)]
        [SerializeField] private int _damageValue = 1;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IDamage>(out IDamage target))
            {
                target.TakeDamage(_damageValue);
            }
        }
    }
}