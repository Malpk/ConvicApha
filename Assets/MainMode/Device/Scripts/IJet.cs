using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public interface IJet : ISetAttack
    {
        public void SetMode(bool mode);
    }
}