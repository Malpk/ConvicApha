using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class Ring : MonoBehaviour
    {
        [SerializeField] private bool _inversMode;

        private bool _visable = false;
        private List<TermPatern> _ternList = new List<TermPatern>();

        public bool invetsMode => _inversMode;

        public void OnVisableTile(bool value)
        {
            _visable = true;
            foreach (var tile in _ternList)
            {
                tile.SetMode(value);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<TermPatern>(out TermPatern tern))
            {
                if (!_inversMode)
                    TurnOff(tern);
                else
                    TurnOn(tern);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<TermPatern>(out TermPatern tern))
            {
                if (!_inversMode)
                    TurnOn(tern);
                else
                    TurnOff(tern);
            }
        }
        private void TurnOn(TermPatern tern)
        {
            if(_visable)
                tern.TurnOn(TernState.Fire);
            _ternList.Add(tern);
        }
        private void TurnOff(TermPatern tern)
        {
            if (_visable)
                tern.TurnOff();
            _ternList.Remove(tern);
        }
    }
}