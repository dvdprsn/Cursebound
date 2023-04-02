using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image healthBarSprite;
    private Camera cam;
    private float target = 1;
    [SerializeField] private float reduceSpeed = 2;


    private void Start()
    {
        cam = Camera.main;
    }
    // Start is called before the first frame update
    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        healthBarSprite.fillAmount = Mathf.MoveTowards(healthBarSprite.fillAmount, target, reduceSpeed * Time.deltaTime);
    }
}
