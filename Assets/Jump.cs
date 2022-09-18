using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jump : StateMachineBehaviour
{
    private Transform player;
    private int i;
    private NavMeshAgent agent;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private int playerDetectingFrames;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 0;
        animator.GetComponent<NavMeshAgent>().enabled = false;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 dirToPlayer =  player.position - animator.transform.position;
        Vector3 tr = (dirToPlayer * Time.deltaTime * jumpSpeed);
        Vector3 pos = animator.transform.position;
        animator.transform.position = new Vector3(tr.x + pos.x, tr.y + pos.y, pos.z);
        if (RayToPlayer(animator))
        {
            i++;
            if (i > playerDetectingFrames)
            {
                animator.SetBool("Chase", true);
            }
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<NavMeshAgent>().enabled = true;
        animator.SetBool("Jump", false);
    }
    private bool RayToPlayer(Animator animator)
    {
        Vector3 directionToPlayer = player.position - animator.transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(animator.transform.position,
            directionToPlayer, 1000);

        return hit2D.collider != null && hit2D.collider.gameObject.tag == "Player";
    }
}
