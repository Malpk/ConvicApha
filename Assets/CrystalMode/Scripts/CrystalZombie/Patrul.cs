using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrul : StateMachineBehaviour
{
    private float time;
    private NavMeshAgent agent;
    private Transform currentPoint;
    private List<Transform> points = new List<Transform>();
    private Transform player;
    [SerializeField] private float patrulSpeed;
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
        float distance = Vector3.Distance(animator.transform.position, currentPoint.transform.position);
        if (distance < 5)
        {
            animator.SetBool("Patrul", false);
        }

        if (time > 30)
        {
            animator.SetBool("Patrul", false);
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
        int rand = Random.Range(0, points.Count);
        agent.SetDestination(points[rand].position);
        currentPoint = points[rand];
    }
    

    private bool RayToPlayer(Animator animator)
    {
        Vector3 directionToPlayer = player.position - animator.transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(animator.transform.position + directionToPlayer.normalized, directionToPlayer, 100);
        Debug.DrawRay(animator.transform.position + directionToPlayer.normalized, directionToPlayer, Color.yellow);
        if (hit2D.collider != null)
        {
            if (hit2D.collider.gameObject.CompareTag("Player") || hit2D.collider.GetComponent<Jeff>())
            {
                return true;
            }
        }

        return false;
    }
}
