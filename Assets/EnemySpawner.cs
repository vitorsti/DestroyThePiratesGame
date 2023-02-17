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
    float spawnRate;
    public static EnemySpawner instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //instancesLimit = RandomWaveNumber();
        spawnRate = PlayerPrefs.GetFloat("SpawnRate", 1.5f);
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
                pool.Add(inAction[i]);
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
            //Round Over
        }
    }

    IEnumerator EnableEnemie()
    {
        int index = waiting.Count - 1;

        while (index > -1)
        {
            Debug.Log(index);
            yield return new WaitForSeconds(spawnRate);
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
