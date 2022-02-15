using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Underworld
{
    public interface IRead
    {
        public bool[,] ReadImage(Texture2D texture);
    }
}