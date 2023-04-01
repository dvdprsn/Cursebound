using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private float difficulty = 1.0f;
    public Transform spawnPoint;
    public GameObject prefab;
    public int numToSpawn = 2;
    GameObject[] doors;
    GameObject[] enemies;
    PlayerStatus stats;
    private void Awake()
    {
        doors = GameObject.FindGameObjectsWithTag("Door");
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            stats.SetDifficulty(difficulty);
            foreach (GameObject door in doors) door.SetActive(true);

            // If the room is not cleared entirely it will respawn the room 
            // if (numToSpawn != 0) numToSpawn = Random.Range(numToSpawn, numToSpawn + 3);

            //Maybe apply difficulty here and round up?
            for (int x = 0; x < numToSpawn; x++)
            {
                Vector3 var = spawnPoint.position;
                var.x += Random.Range(0, -7);
                var.z += Random.Range(0, 7);
                GameObject g = Instantiate(prefab, var, spawnPoint.rotation);
                AIStat stats = g.GetComponent<AIStat>();
                stats.ChangeDifficulty(difficulty);
            }   
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            //if(enemies.Length != numToSpawn) numToSpawn = enemies.Length;
            foreach(GameObject g in enemies) Destroy(g);

        }

    }
    private void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            foreach (GameObject door in doors) door.SetActive(false);
        }
    }
}
