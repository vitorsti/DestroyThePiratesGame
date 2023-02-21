using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : Enemy
{
    [SerializeField]
    //State state;

    private void Start()
    {
        state = State.chase;
    }

    private void FixedUpdate()
    {

        switch (state)
        {

            case State.chase:
                float dis = Vector2.Distance(currentPosition, playerPosition);
                //float rotationDistance = dis;
                currentPosition = transform.position;
                playerPosition = PlayerController.instance.transform.position;
                if (dis > minDistance)
                {

                    MoveToPlayer();
                    LookAtPlayer();
                    if (DetectObstacle())
                        state = State.stopMove;
                }
                break;
            case State.stopMove:
                //newDirection = GetPositon();
                //Debug.Log(newDirection);
                //state = State.none;
                state = State.avoidObstacle;
                break;
            case State.avoidObstacle:
                currentPosition = transform.position;
                AvoidObstacle();
                //state = State.none;
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
