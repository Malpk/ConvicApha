using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    [System.Serializable]
    public class MessangeElement
    {
        [SerializeField] private ModeTypeNew _type;
        [SerializeField] private string _messange;

        public ModeTypeNew type => _type;
        public string messange => _messange;
    }
}