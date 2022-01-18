using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwitchMode
{
    [System.Serializable]
    public class Mode
    {
        [SerializeField] private string _modeName;
        [SerializeField] private float _delay;
        [SerializeField] private GameObject _mode;

        public float delay => _delay;
        public GameObject mode => _mode;
    }
}