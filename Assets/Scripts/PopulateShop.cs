using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateShop : MonoBehaviour
{
    // Start is called before the first frame update
    private Canvas c;
    private Transform container;
    private Transform statContainer;
    private Transform btn;
    private PlayerStatus stats;
    private void Awake()
    {
        c = GetComponent<Canvas>();
        c.enabled = false;
        container = transform.Find("container");
        btn = container.Find("Button");
        btn.gameObject.SetActive(false);

        statContainer = transform.Find("Stats");
        stats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

    }
    private void Start()
    {
        CreateButton("Health +10", 5, 0);
        CreateButton("Dmg Mul +0.50", 30, 1);
        CreateButton("Test2", 3, 2);
        CreateButton("Test2", 3, 3);
        CreateButton("Test2", 3, 4);

    }
    public void CreateButton(string text, int price, int idx)
    {
        Transform shopTransform = Instantiate(btn, container);
        shopTransform.gameObject.SetActive(true);
        RectTransform shopRectTransform = shopTransform.GetComponent<RectTransform>();

        float height = 30f;
        shopRectTransform.anchoredPosition = new Vector2(0, -height * idx);

        shopTransform.Find("itemName").GetComponent<TextMeshProUGUI>().SetText(text);
        shopTransform.Find("itemCost").GetComponent<TextMeshProUGUI>().SetText(price.ToString());

    }

    public void UpdateStats()
    {
        statContainer.Find("MaxHealth").GetComponent<TextMeshProUGUI>().SetText("Max Health => " + stats.MaxHealth.ToString());
        statContainer.Find("DmgMul").GetComponent<TextMeshProUGUI>().SetText("Dmg Mul => " + stats.DmgMul.ToString());
    }

    private void Update()
    {
        if(c.enabled)
        {
            UpdateStats();
        }
    }

}
