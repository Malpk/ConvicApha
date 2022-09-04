using System;
using System.Collections;
using System.Collections.Generic;
using MainMode;
using UnityEngine;
using UnityEngine.AI;
using NavMeshBuilder = UnityEditor.AI.NavMeshBuilder;
using Random = UnityEngine.Random;

public class Zombie : MonoBehaviour
{
   private NavMeshAgent agent;
 
   private void Start()
   {
      agent = gameObject.GetComponent<NavMeshAgent>();
      agent.updateRotation = false;
      agent.updateUpAxis = false;
   }

   private void Update()
   {
      RotateToDirection();
   }

   private void RotateToDirection()
   {
      Vector2 velocity = agent.velocity;
      float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90;
      transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
   }
   
}
