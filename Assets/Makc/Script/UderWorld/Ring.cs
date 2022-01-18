using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private bool _inversMode;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<TermPatern>(out TermPatern tern))
            {
                if (!_inversMode)
                    tern.TurnOff();
                else
                    tern.TurnOn(TernState.Fire);
            }
    }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<TermPatern>(out TermPatern tern))
            {
                if (!_inversMode)
                    tern.TurnOn(TernState.Fire);
                else
                    tern.TurnOff();
            }
        }
    }
}