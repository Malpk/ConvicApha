using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class HandTermTile : TermTile
    {
        #region Work Tile
        public void Activate(FireState state)
        {
            tileAnimator.SetBool("Activate", true);
            isDangerMode = true;
            fire.Activate(state);
        }
        #endregion
    }
}