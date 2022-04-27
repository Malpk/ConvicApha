using UnityEngine;
using System.Collections.Generic;

namespace Underworld.Editor
{
    public class Sequnce
    {
        public List<UnderWorldNode> Elements;

        public Sequnce()
        {
            Elements = new List<UnderWorldNode>();
            Elements.Add(new UnderWorldNode(Vector2.zero,ModeTypeNew.BaseMode,true));
        }
        public int Count => Elements.Count;
    }
}