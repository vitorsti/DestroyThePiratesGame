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

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        myHealth = GetComponent<HealthManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //Player movement

        if (Input.GetKey(KeyCode.W))
        {
            //move foward
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }

        float horizontal;
        horizontal = Input.GetAxis("Horizontal") * rotationSpeed;
        horizontal *= Time.deltaTime;
        //rotate
        transform.Rotate(0, 0, -horizontal, Space.World);

        //

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
        GameObject deathExplosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
        Destroy(deathExplosion, deathExplosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
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
        yield return new WaitForSeconds(singleFireRate);
        singleFireEnable = true;
    }

    IEnumerator EnableMultipleFire()
    {
        yield return new WaitForSeconds(multipleFireRate);
        multipleFireEnable = true;
    }
}
