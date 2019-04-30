using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryReaction : MonoBehaviour {

    public Item item;

    private Inventory inventory;

	// Use this for initialization
	void Start () {
        inventory = FindObjectOfType<Inventory>();
	}
	
    void AddItemToInventory()
    {
        inventory.AddItem(item);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag.Equals("Player"))
        {
            inventory.AddItem(item);
            Destroy(gameObject);
        }
    }
}
