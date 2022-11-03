using UnityEngine;

public class Attack : StateMachineBehaviour
{
    [SerializeField] private int attackValue;
    [SerializeField] private DamageInfo attackInfo;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float stopAttackingDistance;
    private Transform player;
    private float time;
    
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        time = 2;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        time += Time.deltaTime;
        float distance = Vector2.Distance(animator.transform.position, player.position);
        if (distance > stopAttackingDistance)
        {
            animator.SetBool("Chase", true);
        }

        if (time > attackCoolDown)
        {
            Debug.Log("attacked");
            time = 0;
            player.GetComponent<Player>().TakeDamage(attackValue, attackInfo);
        }
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
}