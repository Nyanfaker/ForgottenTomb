using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSword : MonoBehaviour {

    public Item sword;
    public string text;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (InventoryController.instance.CheckItem(6) &&
            InventoryController.instance.CheckItem(7) &&
            InventoryController.instance.CheckItem(8))
        {
            InventoryController.instance.DeleteItem(6);
            InventoryController.instance.DeleteItem(7);
            InventoryController.instance.DeleteItem(8);
            InventoryController.instance.AddNewItem(sword);
            FindObjectOfType<BlockPanel>().Texting(text);
        }
    }

}
