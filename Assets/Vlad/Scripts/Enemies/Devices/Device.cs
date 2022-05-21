using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BaseMode
{
    public abstract class Device : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _effectPrefab;
    }
}