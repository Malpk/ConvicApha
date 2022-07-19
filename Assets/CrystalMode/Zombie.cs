using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour
{
   private NavMeshAgent agent;
   private void Start()
   {
      agent = gameObject.GetComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updateUpAxis = false;
      Physics2D.IgnoreLayerCollision(2, 2);
      Physics.IgnoreLayerCollision(2, 2);
   }
}
