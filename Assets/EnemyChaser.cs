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
                float dis = Vector2.Distance(currentPosition, playerPosition);

                currentPosition = transform.position;
                playerPosition = PlayerController.instance.transform.position;
                if (dis > minDistance)
                {
                    LookAtPlayer();
                    MoveToPlayer();
                }
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


}
