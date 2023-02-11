using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float rotateSpeed;
    
    public float collisionDamage;

    HealthManager myHealth;

    Vector2 currentPosition;
    Vector2 playerPosition;

    public float minDistance;
    // Start is called before the first frame update
    void Awake()
    {
        myHealth = GetComponent<HealthManager>();
    }

    public virtual void MoveToPlayer()
    {
        float dis = Vector2.Distance(currentPosition, playerPosition);

        if (dis > minDistance)
            transform.position = Vector2.MoveTowards(currentPosition, playerPosition, speed * Time.deltaTime);
    }

    public virtual void GetPlayerPosition()
    {
        currentPosition = transform.position;
        playerPosition = PlayerController.instance.transform.position;
    }

    public virtual void LookAtPlayer()
    {
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var offset = 90;
        transform.rotation = Quaternion.Euler(Vector3.forward * rotateSpeed * (angle + offset));
    }
    public void TakeDamage(float damageToTake)
    {
        myHealth.RemoveHealth(damageToTake);

        if (myHealth.health == 0)
            EnemyDeath();
    }

    public virtual void EnemyDeath()
    {

        Destroy(this.gameObject);
    }
}
