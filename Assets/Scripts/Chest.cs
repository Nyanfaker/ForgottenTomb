using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObj {

    public Item chestItem;
    public Sprite closedChest;
    public Sprite openedChest;
    SpriteRenderer sr;
    bool isEmpty = false;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedChest;
    }

    public override void Interacte()
    {
        if (!isEmpty)
        {
            InventoryController.instance.AddNewItem(OpenChest());
            GameManager.instance.Massage(chestItem);
            isEmpty = true;
        }
    }

    public Item OpenChest()
    {
        sr.sprite = openedChest;
        return chestItem;
    }
}
