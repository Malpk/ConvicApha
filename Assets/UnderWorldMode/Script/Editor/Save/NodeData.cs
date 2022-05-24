using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld.Save
{
    [System.Serializable]
    public class NodeData : ScriptableObject
    {
        [field: SerializeField] public int nodeID { get; set; }
        [field: SerializeField] public int seqcunceID { get; set; }
        [field: SerializeField] public int nextPosition { get; set; }
        [field: SerializeField] public string settingSerialize { get; set; }
        [field: SerializeField] public Vector2 position { get; set; }
        [field: SerializeField] public ModeTypeNew type { get; set; }
    }
}