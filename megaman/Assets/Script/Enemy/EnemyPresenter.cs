
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenter : MonoBehaviour, IDamageable
{
    public int hp = 3;

    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
