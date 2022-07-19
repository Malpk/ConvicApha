using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieIdle : StateMachineBehaviour
{
    private float time; 
    private int idleTime;
    private Transform player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        idleTime = Random.Range(4, 8);
        time = 0;
    }

 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        if (time > idleTime)
        {
            animator.SetBool("Patrul", true);
        }
        
        if (RayToPlayer(animator))
        {
            animator.SetBool("Chase", true);
        }
    }

  
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time = 0;
    }

    private bool RayToPlayer(Animator animator)
    {
        Vector3 directionToPlayer = player.position - animator.transform.position;
        RaycastHit2D hit2D = Physics2D.Raycast(animator.transform.position + directionToPlayer.normalized, directionToPlayer, 100);
        Debug.DrawRay(animator.transform.position + directionToPlayer.normalized, directionToPlayer, Color.yellow);
        if (hit2D.collider != null)
        {
            return hit2D.collider.gameObject.CompareTag("Player") || hit2D.collider.GetComponent<Jeff>(); 
        }
        return false;
    }
    
}
