using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jump : StateMachineBehaviour
{
    private Transform player;
    private NavMeshAgent agent;
    private float time;
    private float distanceToPlayer;
    private Vector2 oldPosPlayer;
    [SerializeField] private float jumpSpeed;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 0;
        animator.GetComponent<NavMeshAgent>().enabled = false;
        time = 0;
        oldPosPlayer = player.position;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distanceToPlayer = Vector2.Distance(animator.transform.position, oldPosPlayer);
        time += Time.deltaTime;
        jumpSpeed += Time.deltaTime * 2;
        
        TranslateToPlayer(animator);
        RotateToPlayer(animator);
        if (distanceToPlayer < 0.3 || time >  3)
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
        Vector2 dirToPlayer = oldPosPlayer - (Vector2)animator.transform.position;
        Vector2 dirToPlayer2 = (dirToPlayer * Time.deltaTime * jumpSpeed);
        animator.transform.position = dirToPlayer2 + (Vector2)animator.transform.position;
    }
    private void RotateToPlayer(Animator animator)
    {
        Vector2 dirToPlayer = oldPosPlayer - (Vector2)animator.transform.position;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, q, Time.deltaTime * 5);
    }
}
