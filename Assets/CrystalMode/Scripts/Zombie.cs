using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
   private NavMeshAgent agent;
   public GameObject prefab;

   [SerializeField] private float rotationSmoothness;

   private void Start()
   {
      if (gameObject.GetComponent<NavMeshAgent>())
      {
         agent = gameObject.GetComponent<NavMeshAgent>();
         agent.updateRotation = false;
         agent.updateUpAxis = false;
      }
   }

   private void Update()
   {
      RotateToDirection();
   }
   
   private void RotateToDirection()
   {
      if (agent == null || agent.velocity == Vector3.zero )
      {
         return;
      }
      Vector2 velocity = agent.velocity;
      float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg + 90;
      Quaternion smoothRot = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSmoothness);
      transform.rotation = smoothRot;
   }

   public void Kill()
   {
      ZombieSpawner zombieSpawner = GameObject.FindGameObjectWithTag("ZombieSpawner").GetComponent<ZombieSpawner>();
      zombieSpawner.SpawnEnemyVoid(20, prefab);
      Destroy(gameObject);
   }
   
}
