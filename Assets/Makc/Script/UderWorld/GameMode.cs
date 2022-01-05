using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Underworld
{
    public abstract class GameMode : MonoBehaviour
    {
        private void Update()
        {
            ModeUpdate();
        }
        protected abstract void ModeUpdate();
    }
}