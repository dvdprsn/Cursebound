using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopButton : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private PlayerStatus stats;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStatus>();
    }

    public void PurchaseHealth(int cost)
    {
        if (stats.Souls >= cost)
        {
            stats.AddHealth(10);
            stats.RemoveSouls(cost);
        } else
        {
            Debug.Log("Not Enough Souls to Purchase!");
        }

    }
    public void PurchaseDmgMul(int cost)
    {
        if (stats.Souls >= cost)
        {
            stats.AddDmgMul(0.5f);
            stats.RemoveSouls(cost);
        }
        else
        {
            Debug.Log("Not Enough Souls to Purchase!");
        }
    }
    public void PurchaseSoulMul(int cost)
    {
        if (stats.Souls >= cost)
        {
            stats.AddSoulMul(0.5f);
            stats.RemoveSouls(cost);
        }
        else
        {
            Debug.Log("Not Enough Souls to Purchase!");
        }
    }
    public void PurchaseMaxMana(int cost)
    {
        if (stats.Souls >= cost)
        {
            stats.AddMaxMana(10);
            stats.RemoveSouls(cost);
        }
        else
        {
            Debug.Log("Not Enough Souls to Purchase!");
        }
    }

    public void PurchaseManaRate(int cost)
    {
        if (stats.Souls >= cost)
        {
            stats.AddManaRate(0.2f);
            stats.RemoveSouls(cost);
        }
        else
        {
            Debug.Log("Not Enough Souls to Purchase!");
        }
    }
    public void OnButtonPress(Transform btn)
    {
        string name = btn.Find("itemName").GetComponent<TextMeshProUGUI>().GetParsedText();
        int cost = int.Parse(btn.Find("itemCost").GetComponent<TextMeshProUGUI>().GetParsedText());

        if(name.Contains("Max Health"))
        {
            PurchaseHealth(cost);
        }
        if(name.Contains("Dmg Mul"))
        {
            PurchaseDmgMul(cost);
        }
        if(name.Contains("Soul Mul"))
        {
            PurchaseSoulMul(cost);
        }
        if(name.Contains("Max Mana"))
        {
            PurchaseMaxMana(cost);
        }
        if(name.Contains("Mana Rate"))
        {
            PurchaseManaRate(cost);
        }
    }
    public void OnExit()
    {
        stats.Alive();
    }
}
