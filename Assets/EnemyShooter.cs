using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    [SerializeField]
    //State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.start;
        //state = State.chase;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentPosition = transform.position;
        playerPosition = PlayerController.instance.transform.position;
        float dis = Vector2.Distance(currentPosition, playerPosition);

        switch (state)
        {
            case State.start:

                state = State.chase;
                break;
            case State.chase:



                if (dis > minDistance)
                {
                    LookAtPlayer();
                    MoveToPlayer();
                    if (DetectObstacle())
                        state = State.stopMove;
                }
                else
                {
                    state = State.shoot;
                    fire = true;
                }

                break;
            case State.shoot:
                LookAtPlayer();
                Shoot();
                if (dis > minDistance)
                    state = State.chase;
                break;
            case State.stopMove:
                //newDirection = GetPositon();
                //Debug.Log(newDirection);
                //state = State.none;
                state = State.avoidObstacle;
                break;
            case State.avoidObstacle:
                AvoidObstacle();
                //state = State.chase;
                break;
            case State.dead:
                break;
        }


    }
}
