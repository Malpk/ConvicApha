using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jump : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private float time;
    [SerializeField] private float jumpSpeed;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 0;
        animator.GetComponent<NavMeshAgent>().enabled = false;
        time = 0;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        jumpSpeed += Time.deltaTime * 2;
        
        TranslateToPlayer(animator);
        
        float distanceToPlayer = Vector2.Distance(animator.transform.position, player.position);
        if (distanceToPlayer < 0.3 || time > 4)
        {
            animator.SetBool("Attack", true);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<NavMeshAgent>().enabled = true;
        animator.SetBool("Jump", false);
    }

    private void TranslateToPlayer(Animator animator) 
    {
        Vector2 dirToPlayer =  player.position - animator.transform.position;
        Vector2 tr = (dirToPlayer * Time.deltaTime * jumpSpeed);
        Vector2 pos = animator.transform.position;
        animator.transform.position = new Vector2(tr.x + pos.x, tr.y + pos.y);
    }
}
