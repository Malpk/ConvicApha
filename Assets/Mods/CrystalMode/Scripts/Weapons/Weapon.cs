using System;
using Unity.Mathematics;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private float attackCoolDown;
    
    private float timeSinceAttack;
    
    protected abstract void Shoot();

    public abstract void Reload();

    public void TryShoot()
    {
        if ( timeSinceAttack > attackCoolDown)
        {
            Shoot();
            timeSinceAttack = 0;
        }
    }
    

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
    }

    protected void InstAndAddForce(GameObject bullet, float speed)
    {
        var crystal = Instantiate(bullet, transform.position + transform.up, quaternion.identity);
        Vector2 dir = transform.parent.transform.up;
        crystal.GetComponent<Rigidbody2D>().AddForce(dir * speed);
    }
}
