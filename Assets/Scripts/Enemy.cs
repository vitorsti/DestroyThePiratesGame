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
    Rigidbody2D rb;

    public enum State { none, chase, shoot };
    // Start is called before the first frame update
    void Awake()
    {
        myHealth = GetComponent<HealthManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    public virtual void MoveToPlayer()
    {

        Vector2 direction = (playerPosition - currentPosition).normalized;
        //direction.Normalize();
        rb.AddForce(direction * speed * Time.deltaTime, ForceMode2D.Force);
        //transform.position = Vector2.MoveTowards(currentPosition, playerPosition, speed * Time.deltaTime);
    }

    public virtual void LookAtPlayer()
    {
        /*Vector2 direction = (playerPosition - currentPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        
        rb.rotation = angle;*/

        Vector3 direction = playerPosition - currentPosition;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float targetRotation = angle - 90;

        rb.rotation = Mathf.Lerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime);


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
