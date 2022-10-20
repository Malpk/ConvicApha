using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerChase : StateMachineBehaviour
{
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float stopChaseDistance;
    [SerializeField] private float startAttackDistance;
    private Transform player;
    private float coolDownToPatrul = 2f;
    private float time;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        time = 0;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        float distanceToPlayer = Vector2.Distance(player.position, animator.transform.position);
        if (distanceToPlayer > stopChaseDistance && time > coolDownToPatrul)
        {
            animator.SetBool("Patrul", true);
        }

        if (distanceToPlayer < startAttackDistance)
        {
            animator.SetBool("Attack", true);
        }
        
        RotateToPlayer(animator);
        TranslateToPlayer(animator);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Chase", false);
    }
    private void TranslateToPlayer(Animator animator)
    {
        animator.transform.position += -animator.transform.up * Time.deltaTime * chaseSpeed;
    }
    private void RotateToPlayer(Animator animator)
    {
        Vector2 dirToPlayer = (Vector2)player.position - (Vector2)animator.transform.position;
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, q, Time.deltaTime * 3);
    }
}
