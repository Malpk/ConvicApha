using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public interface IJet : ISetAttack
    {
        public bool IsActive { get; }
        public void SetMode(bool mode);
    }
}