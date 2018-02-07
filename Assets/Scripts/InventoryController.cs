using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour {

    public static InventoryController instance;

    public Item[] startItems;
    [HideInInspector]
    public List<Item> inventoryItems;
    public ItemSlot prefabSlot;
    ItemSlot[] invSlots;

    static Item currentItem;
    static ItemSlot currentSlot;
    static ItemSlot checkSlot;

    Text nameItem;
    Text property;
    SpriteRenderer weapon;
    Player player;
    Button button;
    Text buttonText;
    Transform inventory;
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        nameItem = GameObject.Find("NameObj").GetComponent<Text>();
        property = GameObject.Find("Property").GetComponent<Text>();
        weapon = GameObject.Find("Weapon").GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        button = GameObject.Find("UseButton").GetComponent<Button>();
        buttonText = button.transform.Find("Text").GetComponent<Text>();
        inventory = GameObject.Find("Content").GetComponent<Transform>();
        UnselectItem();
    }

    public void SelectItem(ItemSlot itemSlot)
    {
        nameItem.gameObject.SetActive(true);
        property.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        if (currentSlot != null)
        {
            currentSlot.isSelect = false;
        }
        currentItem = itemSlot.item;
        currentSlot = itemSlot;
        nameItem.text = "\n" + currentItem.itemName;
        if (currentItem.type == Item.Type.Potion)
        {
            property.text = "Heal: " + currentItem.effect.ToString() + "\n\n\n";
            buttonText.text = "Use";
        }
        else if (currentItem.type == Item.Type.Weapon)
        {
            property.text = "Damage: " + currentItem.effect.ToString() + "\n\n\n";
            buttonText.text = "Equip";
        } else
        {
            button.gameObject.SetActive(false);
            property.text = "   Something meaningful. Maybe.";
        }
    }

    public void UnselectItem()
    {
        nameItem.gameObject.SetActive(false);
        property.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        currentItem = null;
        currentSlot = null;
    }

    public void EquipWeapon()
    {
        if (currentItem.type == Item.Type.Weapon)
        {
            player.damage = currentItem.effect;
            weapon.sprite = currentItem.weaponInHand;
            if (checkSlot != null)
            {
                checkSlot.check.gameObject.SetActive(false);
            }
            checkSlot = currentSlot;
            checkSlot.check.gameObject.SetActive(true);
        }
    }

    public void DrinkPotion()
    {
        if (currentItem.type == Item.Type.Potion)
        {
            if (currentItem.itemName == "Health Potion")
            {
                player.currentHealth = (player.currentHealth + currentItem.effect) > player.health ? player.health : player.currentHealth + currentItem.effect;
            }
            else if (currentItem.itemName == "Mana Potion")
            {
                player.currentMana = (player.currentMana + currentItem.effect) > player.mana ? player.mana : player.currentMana + currentItem.effect;
            }
            inventoryItems.Remove(currentItem);
            GameManager.instance.RefreshStat();
            currentSlot.amount -= 1;
            currentSlot.amountText.text = currentSlot.amount.ToString() + "\n";
            if (currentSlot.amount == 0)
            {
                currentSlot.DestroySlot();
                UnselectItem();
            }
        }
    }

    public void AddNewItem(Item item)
    {
        inventoryItems.Add(item);
        AddItem(item);
    }

    public void AddItemFromInventory()
    {
        foreach(Item item in inventoryItems)
        {
            AddItem(item);
        }
    }

    public void AddStartItems()
    {
        foreach (Item item in startItems)
        {
            AddNewItem(item);
        }
    }

    void AddItem(Item item)
    {
        if (item.isScribbling)
        {
            invSlots = inventory.GetComponentsInChildren<ItemSlot>();
            for (int i = 0; i < invSlots.Length; i++)
            {
                if (invSlots[i].item == item)
                {
                    invSlots[i].amount += 1;
                    invSlots[i].amountText.text = invSlots[i].amount.ToString() + "\n";
                    return;
                }
            }
            ItemSlot itemSlot = Instantiate(prefabSlot, inventory);
            itemSlot.item = item;
            itemSlot.UpdateSlot();
        }
        else
        {
            ItemSlot itemSlot = Instantiate(prefabSlot, inventory);
            itemSlot.item = item;
            itemSlot.UpdateSlot();
        }
    }

    public void DeleteItem(int number)
    {
        foreach(Item invItem in inventoryItems.ToArray())
        {
            if(number == invItem.number)
            {
                inventoryItems.Remove(invItem);
                invSlots = inventory.GetComponentsInChildren<ItemSlot>();
                for (int i = 0; i < invSlots.Length; i++)
                {
                    if (invSlots[i].item.number == number)
                    {
                        invSlots[i].DestroySlot();
                        break;
                    }
                }
                UnselectItem();
            }
        }
    }

    public bool CheckItem(int number)
    {
        foreach (Item invItem in inventoryItems)
        {
            if (invItem.number == number)
            {
                return true;
            }
        }
        return false;
    }
}
