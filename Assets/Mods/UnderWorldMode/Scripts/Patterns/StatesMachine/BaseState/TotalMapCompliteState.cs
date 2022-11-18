using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public class TotalMapCompliteState : BasePatternState
    {
        private readonly float cheakDelay;

        private bool _isConplite;
        private float progress = 0f;

        public System.Func<bool> OnCheakComplite;

        public TotalMapCompliteState(float cheakDelay)
        {
            this.cheakDelay = cheakDelay;
        }

        public override bool IsComplite => _isConplite;

        public override void Start()
        {
            progress = 0f;
        }

        public override bool Update()
        {
            progress += Time.deltaTime / cheakDelay;
            if (progress >= 1)
            {
                progress = 0f;
                _isConplite = OnCheakComplite!= null ? OnCheakComplite.Invoke() : true;
            }
            return !_isConplite; 
        }
    }
}