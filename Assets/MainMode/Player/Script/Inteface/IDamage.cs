using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    public void Dead();
    public void TakeDamage(int damage, AttackInfo type);
}
