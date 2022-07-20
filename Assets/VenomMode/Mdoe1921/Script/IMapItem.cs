using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode.Mode1921
{
    public interface IMapItem
    {
        public void SetMode(bool mode);
        public void SetPosition(Vector2 position);
        public void Delete();
    }
}