using System;
using System.Collections;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyerPatrul : StateMachineBehaviour
{
    [SerializeField] private float patrulSpeed;
    [SerializeField] private float startChaseDistance;
    private List<Transform> points = new List<Transform>();
    private Transform currentPoint;
    private Transform player;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindWithTag("Player").transform;
        GameObject[] pointsObject = GameObject.FindGameObjectsWithTag("Point");
        foreach (var point in pointsObject)
        {
            points.Add(point.transform);
        }
        SetRandomPoint();
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distanceToPlayer = Vector2.Distance(player.position, animator.transform.position);
        if (distanceToPlayer < startChaseDistance)
        {
            animator.SetBool("Chase", true);
        }
        
        float distanceToPoint = Vector2.Distance(currentPoint.position, animator.transform.position);
        if (distanceToPoint < 0.1f)
        {
            SetRandomPoint();
        }
        
        TranslateToPoint(currentPoint, animator);
        RotateToPoint(animator);
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Patrul", false);
    }

    private void TranslateToPoint(Transform point, Animator animator)
    {
        animator.transform.position += -animator.transform.up * Time.deltaTime * patrulSpeed;
    }
    private void SetRandomPoint()
    {
        int rand = Random.Range(0, points.Count);
        currentPoint = points[rand];
    }
    
    private void RotateToPoint(Animator animator)
    {
        Vector2 dirToPoint = (Vector2)currentPoint.position - (Vector2)animator.transform.position;
        float angle = Mathf.Atan2(dirToPoint.y, dirToPoint.x) * Mathf.Rad2Deg + 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, q, Time.deltaTime * 1);
    }
}
