using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTile : MonoBehaviour {

    public Item item;
    SpriteRenderer sr;
    Vector3 rotate = new Vector3(0, 0, -45);

	void Start () {
        //item = GetComponent<Item>();
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = item.itemSprite;
        if (item.type == Item.Type.Weapon)
        {
            sr.transform.eulerAngles = rotate;
        }

    }
	
}
