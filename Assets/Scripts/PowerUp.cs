using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    private PlayerStatus stats;
    public GameObject DmgNumPrefab;
    private Camera cam;
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
        cam = Camera.main;
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        var renderer = GetComponent<Renderer>();

        Vector3 pos = transform.position;
        pos.y += 1;
        var go = Instantiate(DmgNumPrefab, pos, Quaternion.LookRotation(pos - cam.transform.position), transform);

        switch (type)
        {

            case PowerUpType.HealthUp:
                renderer.material.SetColor("_Color", Color.red);
                go.GetComponent<TextMesh>().text = "Health Up +20";
                break;
            case PowerUpType.DmgUp:
                renderer.material.SetColor("_Color", Color.blue);
                go.GetComponent<TextMesh>().text = "Damage Mul +0.5";
                break;
            case PowerUpType.SoulUp:
                renderer.material.SetColor("_Color", Color.green);
                go.GetComponent<TextMesh>().text = "Soul Mul +0.5";
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
