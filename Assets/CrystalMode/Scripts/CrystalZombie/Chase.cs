using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : StateMachineBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float stopChaseDistance;
    [SerializeField] private float startAttackDistance;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;
        player = GameObject.FindWithTag("Player").transform;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance > stopChaseDistance)
        {
            animator.SetBool("Patrul", true);
        }

        if (distance < startAttackDistance)
        {
            animator.SetBool("Attack", true);
            animator.SetBool("Chase", false);
        }
        RotateZombieToPlayer(animator);
        if (!RayToPlayer(animator))
        {
            animator.SetBool("Patrul", true);
            animator.SetBool("Chase", false);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Chase", false);
    }

    private void RotateZombieToPlayer(Animator animator)
    {
        var direction = player.position - animator.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.transform.rotation = Quaternion.Lerp(animator.transform.rotation, rot, 0.01f);
    }

    private bool RayToPlayer(Animator animator)
    {
        Vector3 directionToPlayer = player.position - animator.transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(animator.transform.position + directionToPlayer.normalized, directionToPlayer, 100);
        Debug.DrawRay(animator.transform.position + directionToPlayer.normalized, directionToPlayer, Color.yellow);
        if (hit2D.collider != null)
        {
            return hit2D.collider.GetComponent<Jeff>();
        }

        return false;
    }
}
