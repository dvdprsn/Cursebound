using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject prefab;
    public int numToSpawn = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(numToSpawn != 0)
            {
                numToSpawn = Random.Range(numToSpawn, numToSpawn + 5);
            }
            for (int x = 0; x < numToSpawn; x++)
            {
                Vector3 var = spawnPoint.position;
                var.x += Random.Range(0, -10);
                var.z += Random.Range(0, 10);
                Instantiate(prefab, var, spawnPoint.rotation);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if(enemies.Length != numToSpawn)
            {
                numToSpawn = enemies.Length;
            }
            foreach(GameObject g in enemies)
            {
                Destroy(g);
            }

        }

    }
}
