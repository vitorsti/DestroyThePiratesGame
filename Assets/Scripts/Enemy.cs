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
    public float shootDamage;

    public float minDistance;

    HealthManager myHealth;
    [Header("Debug only do not change value")]
    [SerializeField]
    Vector2 currentPosition;
    [SerializeField]
    Vector2 playerPosition;



    public enum State { none, chase, shoot };
    // Start is called before the first frame update
    void Awake()
    {
        myHealth = GetComponent<HealthManager>();

    }

    public virtual void MoveToPlayer()
    {
        float dis = Vector2.Distance(currentPosition, playerPosition);

        currentPosition = transform.position;
        playerPosition = PlayerController.instance.transform.position;

        if (dis > minDistance)
            transform.position = Vector2.MoveTowards(currentPosition, playerPosition, speed * Time.deltaTime);
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
        GameObject deathExplosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
        Destroy(deathExplosion, deathExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(this.gameObject);
    }
}
