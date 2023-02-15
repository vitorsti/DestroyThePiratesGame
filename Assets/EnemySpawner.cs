using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public List<GameObject> instaces;
    public Transform minTop, maxTop, minDown, maxDown;
    public float instancesLimit = 10f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnEnemy()
    {
        RemoveNullObject();

        if (CheckLimit())
            return;

        float topOrdown = Random.value;
        Debug.Log(topOrdown);

        Vector2 positionToSpawn = Vector2.zero;

        if (topOrdown > 0.5f)
        {
            positionToSpawn = new Vector2(Random.Range(maxTop.position.x, minTop.position.x), maxTop.position.y);
        }
        else
        {
            positionToSpawn = new Vector2(Random.Range(maxDown.position.x, minDown.position.x), maxDown.position.y);
        }
        transform.position = positionToSpawn;
        GameObject instance = Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, Quaternion.identity);
        instaces.Add(instance);
    }

    bool CheckLimit()
    {
        if (instaces.Count == instancesLimit)
            return true;
        else
            return false;
    }
    void RemoveNullObject()
    {
        for (int i = instaces.Count - 1; i >= 0; i--)
        {
            if (instaces[i] == null)
            {
                instaces.RemoveAt(i);
            }

        }
    }
}
