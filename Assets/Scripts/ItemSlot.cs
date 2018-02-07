using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

    public Item item;
    public Image itemImage;
    public GameObject check;
    public Text amountText;

    public int amount = 1;

    Vector3 rotate = new Vector3(0, 0, -45);
    [HideInInspector]
    public bool isSelect;

    void Start () {
        isSelect = false;
        itemImage.sprite = item.itemSprite;
        if (item.type == Item.Type.Weapon)
        {
            itemImage.transform.eulerAngles = rotate;
        } 
        if(item.isScribbling)
        {
            amountText.gameObject.SetActive(true);
            amountText.text = amount.ToString() + "\n";
        }
    }
	
    public void UpdateSlot()
    {
        itemImage.sprite = item.itemSprite;
        if (item.type == Item.Type.Weapon)
        {
            itemImage.transform.eulerAngles = rotate;
        }
        if (item.isScribbling)
        {
            amountText.gameObject.SetActive(true);
            amountText.text = amount.ToString() + "\n";
        }
    }

    public void SetItem()
    {
        if (!isSelect)
        {
            InventoryController.instance.SelectItem(this);
            isSelect = true;
        }
        else
        {
            InventoryController.instance.UnselectItem();
            isSelect = false;
        }
    }

    public void DestroySlot()
    {
        Destroy(gameObject);
    }
}
