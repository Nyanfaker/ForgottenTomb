using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoadManager : MonoBehaviour {

    [System.Serializable]
    public class Location
    {
        public Transform location;
        public int level;
    }

    public static SaveLoadManager instance;

    public Player player;
    
    public List<Location> locations;

    string saveFile;
    public int level = 0;

    private void Awake()
    {
        instance = this;
        saveFile = Application.persistentDataPath + "/save.bin";
    }

    public void Load()
    {
        //saveFile = Application.persistentDataPath + "/save.bin";
        if (File.Exists(saveFile))
        {
            Read();
            GameManager.instance.RefreshStat();
            InventoryController.instance.AddItemFromInventory();
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i].level == level)
                {
                    player.transform.position = locations[i].location.position;
                }
            }
        }
    }

    public void Read()
    {
        BinaryReader r = new BinaryReader(new FileStream(saveFile, FileMode.Open));

        player.health = r.ReadInt32();
        player.mana = r.ReadInt32();
        player.level = r.ReadInt32();
        player.needExp = r.ReadInt32();
        player.currentExp = r.ReadInt32();
        player.currentHealth = r.ReadInt32();
        player.currentMana = r.ReadInt32();
        level = r.ReadInt32();

        int count = r.ReadInt32();
        for (int i = 0; i < count; ++i)
        {
            Item item = ItemLibrary.instance.TakeItemFromLibrary(r.ReadInt32());
            InventoryController.instance.inventoryItems.Add(item);
        }

        r.Close();
    }

    public void Save()
    {
        BinaryWriter w = new BinaryWriter(new FileStream(saveFile, FileMode.OpenOrCreate));
        
        w.Write(player.health);
        w.Write(player.mana);
        w.Write(player.level);
        w.Write(player.needExp);
        w.Write(player.currentExp);
        w.Write(player.currentHealth);
        w.Write(player.currentMana);
        w.Write(level);

        w.Write(InventoryController.instance.inventoryItems.Count);
        foreach(Item item in InventoryController.instance.inventoryItems)
        {
            w.Write(item.number);
        }

        w.Close();
    }
}
