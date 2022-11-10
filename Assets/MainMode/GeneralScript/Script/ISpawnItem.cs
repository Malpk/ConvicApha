using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public interface ISpawnItem
    {
        public GameObject InstateItem(Vector3 position, GameObject item);
    }
}