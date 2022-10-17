using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAttack : StateMachineBehaviour
{
    private Transform player;

    [SerializeField] private float stopAttackDistance;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToPlayer = Vector2.Distance(animator.transform.position, player.position);
        if (distanceToPlayer > stopAttackDistance)
        {
            animator.SetBool("Chase", true);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
}
