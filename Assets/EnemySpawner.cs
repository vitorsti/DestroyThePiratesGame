using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> pool;
    public List<GameObject> waiting, inAction;
    public Transform minTop, maxTop, minDown, maxDown;
    public Transform poolLocation;
    Vector2 positionToSpawn;
    public int instancesLimit = 10;
    public static EnemySpawner instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        instancesLimit = RandomWaveNumber();
        GetEnemiesFromPool();
    }
    void Start()
    {
        StartCoroutine(EnableEnemie());
    }

    void GetEnemiesFromPool()
    {
        float limit = instancesLimit;

        while (limit > 0)
        {
            GameObject instance = pool[Random.Range(0, pool.Count)];

            waiting.Add(instance);
            pool.Remove(instance);
            limit--;
        }
    }
    int RandomWaveNumber()
    {
        int i = Random.Range(5, 11);
        return i;
    }
    Vector2 GetLocation()
    {
        //float limit
        Vector2 location = Vector2.zero;
        float topOrdown = Random.value;

        if (topOrdown > 0.5f)
        {
            location = new Vector2(Random.Range(maxTop.position.x, minTop.position.x), maxTop.position.y);
        }
        else
        {
            location = new Vector2(Random.Range(maxDown.position.x, minDown.position.x), maxDown.position.y);
        }

        return location;
    }

    public void DisableEnemie(GameObject enemie)
    {
        Debug.Log("enemie waiting");
        for (int i = 0; i < inAction.Count; i++)
        {
            if (enemie.name == inAction[i].name)
            {
                inAction[i].GetComponent<Collider2D>().enabled = false;
                inAction[i].GetComponent<Enemy>().enabled = false;

                inAction[i].transform.position = poolLocation.position;
                inAction[i].transform.rotation = Quaternion.identity;
                inAction[i].GetComponent<HealthManager>().ResetHealth();

                inAction.Remove(inAction[i]);
            }
        }

        CheckActiveEnemies();
    }

    void CheckActiveEnemies()
    {
        Debug.Log(inAction.Count);
        if (inAction.Count == 0)
        {
            instancesLimit = RandomWaveNumber();

            GetEnemiesFromPool();

            StartCoroutine(EnableEnemie());
        }
    }

    IEnumerator EnableEnemie()
    {
        int index = waiting.Count - 1;

        while (index > -1)
        {
            Debug.Log(index);
            yield return new WaitForSeconds(1f);
            positionToSpawn = Vector2.zero;
            positionToSpawn = GetLocation();
            waiting[index].transform.position = positionToSpawn;
            waiting[index].GetComponent<Collider2D>().enabled = true;
            waiting[index].GetComponent<Enemy>().enabled = true;
            inAction.Add(waiting[index]);
            waiting.Remove(waiting[index]);
            index--;

        }

        yield return null;
    }

}
