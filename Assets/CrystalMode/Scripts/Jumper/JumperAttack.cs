using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class JumperAttack : StateMachineBehaviour
{
    private bool wasAttack;
    private Transform player;
    private Vector2 oldPosPlayer;
    private float i;
    private bool wasInPlayer;
    private NavMeshAgent agent;
    private float time;
    private float attackFrequency = 1;
    private Vector2 dirToPlayer;
    [SerializeField] private int damageValue;
    [SerializeField] private AttackInfo attackInfo;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 0;
        player = GameObject.FindWithTag("Player").transform;
        oldPosPlayer = player.position;
        dirToPlayer = player.position - animator.transform.position;
        wasInPlayer = false;
        i = 0;
        time = 0;
        wasAttack = false;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        float distanceToOldPlayer = Vector2.Distance(animator.transform.position, oldPosPlayer);
        
        if (distanceToOldPlayer < 0.3f)
        {
            wasInPlayer = true;
        }

        float distanceToPLayer = Vector2.Distance(animator.transform.position, player.position);
        if (distanceToPLayer < 0.3f && !wasAttack)
        {
            wasAttack = true;
            Debug.Log("attacked");
            player.GetComponent<Player>().TakeDamage(damageValue, attackInfo);
        }
        
        if (wasInPlayer)
        {
            i += Time.deltaTime;
        }

        if (i > attackFrequency || time > attackFrequency + 0.2f)
        {
            animator.SetBool("Chase", true);
        }

        if (i < 0.4)
        {
            TranslateToOldPosPlayer(animator);
        }
        RotateToPlayer(animator);
    }

    private void RotateToPlayer(Animator animator)
    {
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, q, Time.deltaTime * 10);
    }

    private void TranslateToOldPosPlayer(Animator animator)
    {
        Vector3 pos = animator.transform.position;
        Vector3 norm = dirToPlayer.normalized * Time.deltaTime * 3;
        animator.transform.position = new Vector2(pos.x + norm.x, pos.y + norm.y);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
    
}
