using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 1.5f;
    [SerializeField]
    float rotationSpeed = 50f;
    [SerializeField]
    float damageToDeal = 10f;
    [SerializeField]
    float singleFireRate = 1;
    [SerializeField]
    float multipleFireRate = 2;

    bool singleFireEnable = true;
    bool multipleFireEnable = true;

    public Vector2 screenBounds;

    HealthManager myHealth;

    public Transform firePosition;
    public Transform[] firePositionSides;
    public static PlayerController instance { get; private set; }
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        myHealth = GetComponent<HealthManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //Player movement

        if (Input.GetKey(KeyCode.W))
        {
            //move foward
            //transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
            rb.AddForce(transform.up * speed * rb.mass * Time.deltaTime, ForceMode2D.Force);
        }

        float horizontal;
        horizontal = Input.GetAxis("Horizontal") * rotationSpeed;
        horizontal *= Time.deltaTime;
        //rotate
        //transform.Rotate(0, 0, -horizontal, Space.World);
        rb.AddTorque(-horizontal, ForceMode2D.Force);
        
    }
    private void Update()
    {
        //Fire projectile front
        if (Input.GetMouseButtonDown(0) && singleFireEnable)
        {

            Fire(firePosition);
            StartCoroutine(EnableSingleFire());
            singleFireEnable = false;
        }

        //fire projectile side
        if (Input.GetMouseButtonDown(1) && multipleFireEnable)
        {

            for (int i = 0; i < firePositionSides.Length; i++)
            {
                Fire(firePositionSides[i]);
                StartCoroutine(EnableMultipleFire());
                multipleFireEnable = false;

            }
        }
    }
    private void LateUpdate()
    {
        Vector3 view = transform.position;
        view.x = Mathf.Clamp(view.x, -screenBounds.x, screenBounds.x);
        view.y = Mathf.Clamp(view.y, -screenBounds.y, screenBounds.y);
        transform.position = view;
    }

    public void PlayerTakeDamage(float damageToTake)
    {
        myHealth.RemoveHealth(damageToTake);
        if (myHealth.health == 0)
        {
            PlayerDeath();
        }

    }

    void PlayerDeath()
    {
        GameObject explosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
        Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(this.gameObject);
    }

    void Fire(Transform position)
    {
        GameObject projectile = Instantiate(Resources.Load<GameObject>("Projectile"), position.position, position.rotation);
        projectile.GetComponent<ProjectileBehavior>().SetDamage(damageToDeal);
        projectile.GetComponent<ProjectileBehavior>().SetEnemyTypePirate();
        projectile.tag = this.tag;
        Debug.Log("firing" + position);
    }
    IEnumerator EnableSingleFire()
    {
        float nextTimeTofire = 1f / singleFireRate;
        yield return new WaitForSeconds(nextTimeTofire);
        singleFireEnable = true;
    }

    IEnumerator EnableMultipleFire()
    {
        float nextTimeTofire = 1f / multipleFireRate;
        yield return new WaitForSeconds(nextTimeTofire);
        multipleFireEnable = true;
    }
}
