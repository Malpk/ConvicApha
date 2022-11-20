using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TotalMapCompliteState : BasePatternState
    {
        private readonly float cheakDelay;

        private bool _isConplite;
        private float _progress = 0f;

        public System.Func<bool> OnCheakComplite;

        public TotalMapCompliteState(float cheakDelay)
        {
            this.cheakDelay = cheakDelay;
        }

        public override bool IsComplite => _isConplite;

        public override void Start()
        {
            _isConplite = false;
            _progress = 0f;
        }

        public override bool Update()
        {
            _progress += Time.deltaTime / cheakDelay;
            if (_progress >= 1)
            {
                _progress = 0f;
                _isConplite = OnCheakComplite!= null ? OnCheakComplite.Invoke() : true;
            }
            return !_isConplite; 
        }
    }
}