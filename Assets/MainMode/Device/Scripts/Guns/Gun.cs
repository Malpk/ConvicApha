using MainMode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainMode
{
    public abstract class Gun : Device
    {
        [SerializeField] protected AttackInfo attackInfo;
    }
}