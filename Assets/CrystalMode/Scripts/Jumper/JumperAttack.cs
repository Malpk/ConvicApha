using UnityEngine;
using UnityEngine.AI;

public class JumperAttack : StateMachineBehaviour
{
    [SerializeField] private float attackReloadTime;
    [SerializeField] private int damageValue;
    [SerializeField] private DamageInfo attackInfo;
    [SerializeField] private float aimStrength;
    [SerializeField] private float attackSpeed;

    private bool wasDealDamage;
    private Transform player;
    private Vector2 oldPosPlayer;
    private float i;
    private bool wasInPlayer;
    private NavMeshAgent agent;
    private float time;
    private Vector2 dirToPlayer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        dirToPlayer = oldPosPlayer - (Vector2)animator.transform.position;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 0;
        player = GameObject.FindWithTag("Player").transform;
        oldPosPlayer = player.position;
        wasInPlayer = false;
        i = 0;
        time = 0;
        wasDealDamage = false;
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
        if (distanceToPLayer < 0.3f && !wasDealDamage)
        {
            float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg + 90;
            animator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            wasDealDamage = true;
            Debug.Log("attacked");
            player.GetComponent<Player>().TakeDamage(damageValue, attackInfo);
        }
        
        if (wasInPlayer)
        {
            i += Time.deltaTime;
        }

        if (i > attackReloadTime || time > attackReloadTime + 0.2f)
        {
            animator.SetBool("Chase", true);
        }

        if (i < 0.3f && Time.time > attackReloadTime)
        {
            TranslateToOldPosPlayer(animator);
        }
        
        if (i == 0)
        {
            RotateToPlayer(animator);
        }
    }
    private void RotateToPlayer(Animator animator)
    {
        float angle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, q, Time.deltaTime * 5);
    }
    private void TranslateToOldPosPlayer(Animator animator)
    {
        oldPosPlayer = Vector2.Lerp(oldPosPlayer, player.position, Time.deltaTime * aimStrength);
        if (i == 0)
        {
            dirToPlayer = oldPosPlayer - (Vector2) animator.transform.position;
        }
        Vector2 norm = dirToPlayer.normalized * Time.deltaTime * attackSpeed;
        animator.transform.position +=  (Vector3)norm;
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Attack", false);
    }
}
