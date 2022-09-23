using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JumperPatrul : StateMachineBehaviour
{
    private float time;
    private NavMeshAgent agent;
    private Transform currentPoint;
    private List<Transform> points = new List<Transform>();
    private Transform player;
    [SerializeField] private float patrulSpeed;
    [SerializeField] private float ableJumpDistance;
    [SerializeField] private float jumpCoolDown;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = patrulSpeed;
        time = 0;
        player = GameObject.FindWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        GameObject[] pointsObject = GameObject.FindGameObjectsWithTag("Point");
        foreach (var VARIABLE in pointsObject)
        {
            points.Add(VARIABLE.transform);
        }
        SetDistancion();
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        float distanceToPoint = Vector2.Distance(animator.transform.position, currentPoint.transform.position);
        
        if (distanceToPoint < 0.11)
        {
            SetDistancion();
        }
        
        float distanceToPlayer = Vector2.Distance(animator.transform.position, player.transform.position);
        
        if (distanceToPlayer < ableJumpDistance && time > jumpCoolDown)
        {
            time = 0;
            animator.SetBool("Jump", true);
        }
        
        if (RayToPlayer(animator))
        {
            animator.SetBool("Chase", true);
        }
    }

   
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Patrul", false);
    }

    private void SetDistancion()
    {
        Transform point = points[Random.Range(0, points.Count)];
        agent.SetDestination(point.position);
        currentPoint = point;
    }
    
    private bool RayToPlayer(Animator animator)
    {
        Vector3 directionToPlayer = player.position - animator.transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(animator.transform.position + directionToPlayer.normalized,
            directionToPlayer, 1000);

        return hit2D.collider != null && hit2D.collider.gameObject.CompareTag("Player");
    }
}