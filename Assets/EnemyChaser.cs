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
        currentPosition = transform.position;
        playerPosition = PlayerController.instance.transform.position;
        float dis = Vector2.Distance(currentPosition, playerPosition);
        switch (state)
        {

            case State.chase:



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
            case State.dead:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != State.dead)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerController.instance.PlayerTakeDamage(collisionDamage);
                myHealth.RemoveHealth(10000);

                if (myHealth.health <= 0)
                {
                    GameObject fire = Instantiate(Resources.Load<GameObject>("FireAnimation"), transform.position, transform.rotation, transform);
                    Destroy(fire, 1f);
                    StartCoroutine(WaitForDeath());
                }
            }
        }
    }


}
