using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRoomRoof : MonoBehaviour
{
    public GameObject roof;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            roof.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            roof.SetActive(true);

        }

    }
}
