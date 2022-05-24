using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Underworld.Editors;

namespace Underworld.Save
{
    public class SeqcunceDate : ScriptableObject
    {
        public string[] parent = null;
        public List<int> ContainNods = new List<int>();

        [field: SerializeField] public int seqcunceID { get; set; }
        [field: SerializeField] public string nameSeqcunce { get; set; }
    }
}
