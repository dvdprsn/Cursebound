using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    enum PowerUpType
    {
        HealthUp,
        DmgUp,
        SoulUp,
        None
    };
    [SerializeField]
    PowerUpType powerType = new PowerUpType();

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


            Vector3 var = spawnPoint.position;

            //Maybe apply difficulty here and round up?
            for (int x = 0; x < numToSpawn; x++)
            {
                var.x += 1;
                var.z += 1;
                GameObject g = Instantiate(prefab, var, spawnPoint.rotation);
                AIStat stats = g.GetComponent<AIStat>();
                stats.ChangeDifficulty(difficulty);
                stats.SetPowerType((int)powerType);
            }   
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
            //Destroy all powerups and enemies if they remain on room exit 
            // Enemies is just a saftey thing
            foreach (GameObject g in enemies) Destroy(g);
            foreach (GameObject g in powerUps) Destroy(g);


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
