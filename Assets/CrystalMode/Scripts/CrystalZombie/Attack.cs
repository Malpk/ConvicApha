using UnityEngine;

public class Attack : StateMachineBehaviour
{
    [SerializeField] private float stopAttackingDistance;
    private Transform player;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance > stopAttackingDistance)
        {
            animator.SetBool("Chase", true);
        }
       // RotateZombieToPlayer(animator);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }

    /*private void RotateZombieToPlayer(Animator animator)
    {
        var direction = player.position - animator.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.transform.rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
    }*/
}