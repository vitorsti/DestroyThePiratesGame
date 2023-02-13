using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : Enemy
{
    [SerializeField]
    State state;
    private void Start()
    {
        state = State.chase;
    }

    private void Update()
    {

        switch (state)
        {

            case State.chase:
                LookAtPlayer();
                MoveToPlayer();
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.instance.PlayerTakeDamage(collisionDamage);
            TakeDamage(10000f);
        }
    }

    /*public override void EnemyDeath()
    {
        GameObject deathExplosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
        Destroy(deathExplosion, deathExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        base.EnemyDeath();
    }*/

}
