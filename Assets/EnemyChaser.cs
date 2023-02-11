using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : Enemy
{

    private void FixedUpdate()
    {
        LookAtPlayer();
        GetPlayerPosition();
        MoveToPlayer();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.instance.PlayerTakeDamage(collisionDamage);
            EnemyDeath();
        }
    }

    public override void EnemyDeath()
    {
        GameObject deathExplosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
        Destroy(deathExplosion, deathExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        base.EnemyDeath();
    }
}
