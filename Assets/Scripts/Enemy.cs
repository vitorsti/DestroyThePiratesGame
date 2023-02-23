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
    public float scoreToAdd;
    public bool fire = false;
    public Transform firePositon;
    internal HealthManager myHealth;
    [Header("Debug only do not change value")]
    [SerializeField]
    public Vector2 currentPosition;
    [SerializeField]
    public Vector2 playerPosition;
    Rigidbody2D rb;

    public Transform raycastPosiotion;

    public float hitDistance;
    LayerMask mask;
    public enum State { dead, chase, shoot, avoidObstacle, stopMove, start };
    public State state;
    [Header("----- Debug -----")]
    [SerializeField]
    bool triggerDeath;
    [SerializeField]
    float damageToDeal;
    [SerializeField]
    bool dealDamage;
    // Start is called before the first frame update
    private void OnValidate()
    {
        if (triggerDeath)
        {
            triggerDeath = false;
            EnemyDeath();

        }

        if (dealDamage)
        {
            dealDamage = false;
            TakeDamage(damageToDeal);
        }
    }
    void Awake()
    {
        myHealth = GetComponent<HealthManager>();
        rb = GetComponent<Rigidbody2D>();

        mask = LayerMask.GetMask("Obstacles");
        fire = false;
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
        Vector2 direction = playerPosition - currentPosition;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = Mathf.Lerp(rb.rotation, angle, rotateSpeed * Time.deltaTime);

        /*Vector3 direction = playerPosition - currentPosition;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float targetRotation = angle - 90;

        rb.rotation = Mathf.Lerp(rb.rotation, targetRotation, rotateSpeed * Time.deltaTime);*/


    }
    public virtual void TakeDamage(float damageToTake)
    {
        myHealth.RemoveHealth(damageToTake);

        if (myHealth.health <= 0)
        {
            state = State.dead;
            GameManager.instance.AddScore(scoreToAdd);
            GameObject fire = Instantiate(Resources.Load<GameObject>("FireAnimation"), transform.position, transform.rotation, transform);
            Destroy(fire, 1f);
            StartCoroutine(WaitForDeath());
        }
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

        EnemySpawner.instance.DisableEnemie(this.gameObject);
        myHealth.ResetHealth();

        //Destroy(this.gameObject);
    }
    internal IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(1.5f);
        EnemyDeath();
    }
    public bool DetectObstacle()
    {

        Debug.DrawRay(raycastPosiotion.position, transform.up * hitDistance, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosiotion.position, transform.up, hitDistance, mask);
        if (hit.collider != null)
        {
            Debug.Log(hit.transform.gameObject.name);
            return true;
        }
        else
        {
            return false;
        }

    }
    public virtual void AvoidObstacle()
    {

       

        RaycastHit2D hit = Physics2D.Raycast(raycastPosiotion.position, transform.up, hitDistance, mask);
        Debug.DrawRay(raycastPosiotion.position, transform.up, Color.red);
        if (hit.collider != null)
        {
            
            Vector2 obstacleDirection = (Vector2)transform.position - hit.point;
            Vector2 tangentDirection = new Vector2(obstacleDirection.y, -obstacleDirection.x).normalized;
            Debug.Log(tangentDirection);
            rb.velocity = tangentDirection * (speed / 2) * Time.deltaTime;
            

        }
        else
        {
            
            state = State.chase;
        }

    }

   
    IEnumerator EnableFire()
    {
        float nextTimeTofire = 1f / fireRate;
        yield return new WaitForSeconds(nextTimeTofire);
        fire = true;
    }
}
