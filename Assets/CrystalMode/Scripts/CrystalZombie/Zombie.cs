using System;
using System.Collections;
using System.Collections.Generic;
using MainMode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Zombie : MonoBehaviour
{
   private NavMeshAgent agent;
   public AttackInfo attackInfo;
 
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

   private void OnCollisionEnter2D(Collision2D col)
   {
      if (col.gameObject.GetComponent<TilemapCollider2D>())
      {
         Debug.Log(col);
      }
   }

   private void RotateToDirection()
   {
      if (agent.velocity == new Vector3(0,0,0))
      {
         return;
      }
      Vector2 velocity = agent.velocity;
      float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90;
      transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
   }
   
}
