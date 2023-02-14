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
    public float fireRate;
    public float minDistance;
    bool fire = true;
    public Transform firePositon;
    HealthManager myHealth;
    [Header("Debug only do not change value")]
    [SerializeField]
    public Vector2 currentPosition;
    [SerializeField]
    public Vector2 playerPosition;


    public enum State { none, chase, shoot };
    // Start is called before the first frame update
    void Awake()
    {
        myHealth = GetComponent<HealthManager>();

    }

    public virtual void MoveToPlayer()
    {

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
    public virtual void Shoot()
    {
        if (fire)
        {
            GameObject projectile = Instantiate(Resources.Load<GameObject>("Projectile"), firePositon.position, firePositon.rotation);
            projectile.GetComponent<ProjectileBehavior>().SetDamage(shootDamage);
            projectile.GetComponent<ProjectileBehavior>().SetEnemyTypePlayer();
            projectile.tag = this.tag;
            fire = false;
            StartCoroutine(EnableFire());
        }

        //Debug.Log("firing" + position);
    }
    public virtual void EnemyDeath()
    {
        GameObject explosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(this.gameObject);
    }

    IEnumerator EnableFire()
    {
        float nextTimeTofire = 1f / fireRate;
        yield return new WaitForSeconds(nextTimeTofire);
        fire = true;
    }
}
