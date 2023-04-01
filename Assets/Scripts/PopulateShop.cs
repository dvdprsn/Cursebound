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
        CreateButton("Max Health +10", 1, 0);
        CreateButton("Dmg Mul +0.5", 1, 1);
        CreateButton("Soul Mul +0.5", 1, 2);
        CreateButton("Max Mana +10", 1, 3);
        CreateButton("Mana Rate +0.2", 1, 4);

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
        statContainer.Find("SoulMul").GetComponent<TextMeshProUGUI>().SetText("Soul Mul => " + stats.SoulMul.ToString());
        statContainer.Find("MaxMana").GetComponent<TextMeshProUGUI>().SetText("Max Mana => " + stats.GetMaxMana.ToString());
        statContainer.Find("ManaRate").GetComponent<TextMeshProUGUI>().SetText("Mana Recharge => " + stats.GetManaRechargeRate.ToString());
        statContainer.Find("Souls").GetComponent<TextMeshProUGUI>().SetText("Souls: " + stats.Souls.ToString());
    }

    private void Update()
    {
        if(c.enabled)
        {
            UpdateStats();
        }
    }

}
