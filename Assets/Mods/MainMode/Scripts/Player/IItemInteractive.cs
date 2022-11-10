using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public interface IItemInteractive
    {
        public bool Interactive(Player player);
    }
}