using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage 
{
    public void Explosion();
    public void TakeDamage(int damage, DamageInfo type);
}
