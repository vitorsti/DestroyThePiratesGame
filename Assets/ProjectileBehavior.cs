using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField]
    float speed;
    float damage;
    enum EnemyType { player, pirateEnemy }
    EnemyType chooseEnemy;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;

    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
    }
    private void LateUpdate()
    {
        Vector3 view = transform.position;

        view.x = Mathf.Clamp(view.x, -player.screenBounds.x, player.screenBounds.x);
        view.y = Mathf.Clamp(view.y, -player.screenBounds.y, player.screenBounds.y);

        if (transform.position.x > view.x || transform.position.x < view.x || transform.position.y > view.y || transform.position.y < view.y)
            Destroy(this.gameObject, 0.5f);
    }

    public void SetEnemyTypePirate()
    {
        chooseEnemy = EnemyType.pirateEnemy;
    }

    public void SetEnemyTypePlayer()
    {
        chooseEnemy = EnemyType.player;
    }
    public void SetDamage(float damageToDeal)
    {
        damage = damageToDeal;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (chooseEnemy)
        {
            case EnemyType.player:
                if (other.gameObject.tag == "Player")
                {
                    GameObject explosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
                    Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                    other.GetComponent<PlayerController>().PlayerTakeDamage(damage);
                    Destroy(this.gameObject);
                }

                break;
            case EnemyType.pirateEnemy:
                if (other.gameObject.tag == "Enemy")
                {
                    GameObject explosion = Instantiate(Resources.Load<GameObject>("Explosion"), transform.position, Quaternion.identity);
                    explosion.transform.localScale.Scale(this.gameObject.transform.localScale);
                    Destroy(explosion, explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
                    other.GetComponent<Enemy>().TakeDamage(damage);
                    Destroy(this.gameObject);
                }

                break;
        }


    }
}
