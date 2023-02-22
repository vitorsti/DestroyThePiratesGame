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
    public enum State { none, chase, shoot, avoidObstacle, stopMove,start };
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
        if (hit.collider != null /*&& hit.transform.tag == "Island" || hit.collider != null && hit.transform.tag == "Enemy"*/)
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

        // Cast four rays in front of the enemy
        //float maxSpeed = speed; // The maximum speed of the enemy
        //float maxSteerForce = 10f; // The maximum steering force
        //float rayDistance = 2f; // The distance of the rays

        // The enemy's rigidbody
        //Vector2 currentVelocity = Vector2.zero;

        //RaycastHit2D[] hits = new RaycastHit2D[4];
        //Vector2[] rayDirections = new Vector2[] {
        //    transform.up,
        //    (transform.up + transform.right).normalized,
        //    (-transform.up + transform.right).normalized,
        //    -transform.up
        //};

        //for (int i = 0; i < hits.Length; i++)
        //{
        //    hits[i] = Physics2D.Raycast(raycastPosiotion.position, rayDirections[i], rayDistance);
        //    Debug.DrawRay(raycastPosiotion.position, rayDirections[i] * rayDistance, Color.red);

        //}

        //// Calculate the steering force to avoid obstacles
        //Vector2 avoidForce = Vector2.zero;
        //for (int i = 0; i < hits.Length; i++)
        //{
        //    if (hits[i].collider != null)
        //    {
        //        // Calculate the distance to the obstacle
        //        float distance = Vector2.Distance(transform.position, hits[i].point);

        //        // Calculate the steering force based on the distance to the obstacle
        //        avoidForce += hits[i].normal * (1 / distance);
        //    }

        //}

        //// Normalize the steering force and apply it to the enemy's current velocity
        //if (avoidForce.magnitude > 0)
        //{
        //    avoidForce = avoidForce.normalized * maxSteerForce;
        //    currentVelocity += avoidForce * Time.fixedDeltaTime;
        //}

        //// Limit the enemy's speed to the maximum speed
        //if (currentVelocity.magnitude > maxSpeed)
        //{
        //    currentVelocity = currentVelocity.normalized * maxSpeed;
        //}

        //// Update the enemy's position based on its current velocity
        //rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
        //Vector2 p1 = position;
        //playerPosition = PlayerController.instance.transform.position;
        //Vector2 p2 = playerPosition;
        //float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) * Mathf.Rad2Deg;
        //rb.rotation = Mathf.Lerp(rb.rotation, angle, rotateSpeed * Time.deltaTime);
        //Debug.Log("Tangente entre os pontos: " + angle);

        RaycastHit2D hit = Physics2D.Raycast(raycastPosiotion.position, transform.up, hitDistance, mask);
        Debug.DrawRay(raycastPosiotion.position, transform.up, Color.red);
        if (hit.collider != null)
        {
            // Se houver obstáculo, desvia dele
            Vector2 obstacleDirection = (Vector2)transform.position - hit.point;
            Vector2 tangentDirection = new Vector2(obstacleDirection.y, -obstacleDirection.x).normalized;
            Debug.Log(tangentDirection);
            rb.velocity = tangentDirection * speed * Time.deltaTime;


        }
        else
        {
            // Se não houver obstáculo, segue em direção ao jogador
            /*Vector2 playerDirection = player.position - transform.position;
            float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            rb.velocity = transform.up * speed;*/
            state = State.chase;
        }

    }
    public bool checkAllTrue()
    {
        int i = 0;
        i++;

        if (i >= 4)
            return true;
        else
            return false;
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
