using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerStatus stats;
    enum PowerUpType
    {
        HealthUp,
        DmgUp,
        SoulUp,
        None
    };
    PowerUpType type = PowerUpType.None;
    public void SetType(int t)
    {
        type = (PowerUpType)t;
    }
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        var renderer = GetComponent<Renderer>();
        switch (type)
        {
            case PowerUpType.HealthUp:
                renderer.material.SetColor("_Color", Color.red);
                break;
            case PowerUpType.DmgUp:
                renderer.material.SetColor("_Color", Color.blue);
                break;
            case PowerUpType.SoulUp:
                renderer.material.SetColor("_Color", Color.green);
                break;
            default:
                Destroy(this.gameObject);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch(type)
            {
                case PowerUpType.HealthUp:
                    stats.GiveTmpHealthBoost(20);
                    break;
                case PowerUpType.DmgUp:
                    stats.GiveTmpDmgBoost(0.5f);
                    break;
                case PowerUpType.SoulUp:
                    stats.GiveTmpSoulBoost(0.5f);
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }

    }
}
