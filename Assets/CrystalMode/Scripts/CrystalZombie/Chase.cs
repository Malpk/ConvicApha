using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    private float time;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float startAttackDistance;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed + Random.Range(0.01f, 0.3f);
        player = GameObject.FindWithTag("Player").transform;
        time = 0;
        float distance = Vector2.Distance(animator.transform.position, player.position);
        if (distance < startAttackDistance && RayToPlayer(animator))
        {
            animator.SetBool("Attack", true);
        }
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector2.Distance(animator.transform.position, player.position);
        if (distance < startAttackDistance && RayToPlayer(animator))
        {
            animator.SetBool("Attack", true);
        }
        
        time += Time.deltaTime;
        agent.SetDestination(player.position);

        if (!RayToPlayer(animator) && time > 1)
        {
            animator.SetBool("Patrul", true);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Chase", false);
    }
    
    private bool RayToPlayer(Animator animator)
    {
        Vector3 directionToPlayer = player.position - animator.transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(animator.transform.position + directionToPlayer.normalized, directionToPlayer, 100);
        
        return hit2D.collider != null && hit2D.collider.gameObject.CompareTag("Player");
    }
}
