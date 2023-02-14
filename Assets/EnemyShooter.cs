using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    [SerializeField]
    State state;
    // Start is called before the first frame update
    void Start()
    {
        state = State.chase;
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector2.Distance(currentPosition, playerPosition);

        currentPosition = transform.position;
        playerPosition = PlayerController.instance.transform.position;

        switch (state)
        {
            case State.chase:

                if (dis > minDistance)
                {
                    LookAtPlayer();
                    MoveToPlayer();

                }
                else
                {
                    state = State.shoot;
                }
                break;
            case State.shoot:
                LookAtPlayer();
                Shoot();
                if (dis > minDistance)
                    state = State.chase;
                break;
        }
    }
}
