using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;

namespace MainMode.Items
{
    public interface ITransport : IMovement 
    {
        public void Exit();
    }
}