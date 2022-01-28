using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class Mode
    {
        [SerializeField] private string _modeName;
        [SerializeField] private float _delay;
        [SerializeField] private ModeType _type;
        [SerializeField] private GameObject _mode;

        public float delay => _delay;
        public ModeType type => _type;
        public GameObject mode => _mode;
    }
}