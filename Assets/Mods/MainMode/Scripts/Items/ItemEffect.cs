using System;
using UnityEngine;


namespace MainMode.Items
{    
    public class ItemEffect : MonoBehaviour,IItemEffect
    {         
        [SerializeField]protected float _duration;
        public float Duration { get => _duration; set => _duration = value; }
        public virtual void UseEffect(Player player) { }
    }
}