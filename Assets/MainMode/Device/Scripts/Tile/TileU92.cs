using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public class TileU92 : Trap
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            SetScreen(collision);
        }
    }
}