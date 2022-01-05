using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Underworld
{
    public class FireAnimation : MonoBehaviour
    {
        public void OnFireDestroy()
        {
            Destroy(gameObject);
        }
    }
}