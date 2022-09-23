using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class JumperAttack : StateMachineBehaviour
{
    private Transform player;
    private Vector2 dirToPlayer;
    private Vector2 oldPosPlayer;
    private float i;
    private bool wasInPlayer;
    [SerializeField] private float attackFrequency = 3;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        oldPosPlayer = player.position;
        dirToPlayer = player.position - animator.transform.position;
        i = 0;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToOldPlayer = Vector2.Distance(animator.transform.position, oldPosPlayer);
        
        if (distanceToOldPlayer < 0.3f)
        {
            wasInPlayer = true;
        }

        if (wasInPlayer)
        {
            i += Time.deltaTime * attackFrequency;
            if (i > 200)
            {
                animator.SetBool("Chase", true);
            }
        }

        Vector3 pos = animator.transform.position;
        Vector3 norm = dirToPlayer.normalized * Time.deltaTime * 5;
        animator.transform.position = new Vector2(pos.x + norm.x, pos.y + norm.y);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //player.GetComponent<Player>().TakeDamage(2, animator.GetComponent<Zombie>().attackInfo);
        animator.SetBool("Attack", false);
    }
    
}
