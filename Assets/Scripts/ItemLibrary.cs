using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLibrary : MonoBehaviour {

    public static ItemLibrary instance;
    public List<Item> library;

    private void Awake()
    {
        instance = this;
    }

    public Item TakeItemFromLibrary(int number)
    {
        for (int i = 0; i < library.Count; i++)
        {
            if (number == library[i].number)
            {
                return library[i];
            }
        }
        return null;
    }
}
