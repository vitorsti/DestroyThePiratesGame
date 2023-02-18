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

    public Transform raycastPosiotion;

    public float hitDistance;

    public enum State { none, chase, shoot, avoidObstacle, stopMove };
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

        EnemySpawner.instance.DisableEnemie(this.gameObject);
        myHealth.ResetHealth();

        //Destroy(this.gameObject);
    }

    public bool DetectObstacle()
    {

        Debug.DrawRay(raycastPosiotion.position, transform.up * hitDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosiotion.position, transform.up, hitDistance);
        if (hit.collider != null && hit.transform.tag == "Island")
        {
            Debug.Log(hit.transform.gameObject.name);
            return true;
        }
        else
        {
            return false;
        }

    }
    public virtual void AvoidObstacle(RaycastHit2D position)
    {

        /*Vector2 rotation = position - currentPosition;
        float angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg + 90f;
        rb.rotation = angle*Time.deltaTime;*/
        //Vector2 normal = position.normal;
        ///Vector2 direction = Vector2.Reflect(transform.right, normal);

        // Rotate the enemy to face the new direction
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

       //Vector3 directionRotation = direction - currentPosition;
        //direction.Normalize();
        //float angle = Mathf.Atan2(directionRotation.y, directionRotation.x) * Mathf.Rad2Deg;
        //float targetRotation = angle - 90;

       // rb.rotation = Mathf.Lerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        // Move the enemy in the new direction
        //rb.velocity = direction * 0.1f;


    }

    public RaycastHit2D GetPositon()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Debug.DrawRay(raycastPosiotion.position, transform.up * hitDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosiotion.position, transform.up, hitDistance);

        //Vector2 normal = hit.normal;
        //Vector2 direction = Vector2.Reflect(transform.right, normal);
        //Debug.DrawRay(normal, direction * hitDistance, Color.red);
        return hit;
    }
    IEnumerator EnableFire()
    {
        float nextTimeTofire = 1f / fireRate;
        yield return new WaitForSeconds(nextTimeTofire);
        fire = true;
    }
}
