using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class Item : ScriptableObject {

    public int number;

	public enum Type { Potion, Weapon, Piece }
    public Type type;
    public string itemName;
    public int effect;
    public bool isScribbling;
    public Sprite itemSprite;
    public Sprite weaponInHand;

}
