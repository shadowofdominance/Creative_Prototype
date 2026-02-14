using UnityEngine;
using UnityEngine.InputSystem;

public class Spawn : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private float xRange = 10;
    private float yPos = 4;
    private float startDelay;
    void Start()
    {
        float spawnInterwal = Random.Range(1.0f, 2.5f);
        InvokeRepeating("RandomObjectSpawner", startDelay, spawnInterwal);
    }
    void RandomObjectSpawner()
    {
        int randomIndex = Random.Range(0, objectPrefabs.Length);

        Vector3 spawnPos = new Vector3(Random.Range(-xRange, xRange), yPos, 0);

        Instantiate(objectPrefabs[randomIndex], spawnPos, objectPrefabs[randomIndex].transform.rotation);
    }
}
