using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private PlayerStatus stats;

    private void Awake()
    {
        stats = player.GetComponent<PlayerStatus>();
    }
    public void OnButtonPress(Transform btn)
    {
        string name = btn.Find("itemName").GetComponent<TextMeshProUGUI>().GetParsedText();
        int cost = int.Parse(btn.Find("itemCost").GetComponent<TextMeshProUGUI>().GetParsedText());

        Debug.Log("Pressed " + name + "Cost: " + cost.ToString());
    }
    public void OnExit()
    {
        stats.Alive();
        Debug.Log("Shop Exit Pressed");
    }
}
