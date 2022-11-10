using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerAttack : StateMachineBehaviour
{
    private Transform player;

    [SerializeField] private float stopAttackDistance;
    [SerializeField] private int damagevalue;
    [SerializeField] private DamageInfo AttackInfo;

    private float timeSinceAttack;
    private float attackCoolDown = 1.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        timeSinceAttack = 1.5f;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeSinceAttack += Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(animator.transform.position, player.position);
        if (distanceToPlayer > stopAttackDistance)
        {
            animator.SetBool("Chase", true);
        }

        if (timeSinceAttack > attackCoolDown)
        {
            player.GetComponent<Player>().TakeDamage(damagevalue, AttackInfo);
            timeSinceAttack = 0;
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
}
