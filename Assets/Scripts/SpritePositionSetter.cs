using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePositionSetter : MonoBehaviour {

    void Awake()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -(int)transform.position.y;
        if (name == "WallUP_2" || name == "Weapon")
        {
            GetComponent<SpriteRenderer>().sortingOrder += 1;
        }
        //if (name == "Player")
        //{
        //    GetComponent<SpriteRenderer>().sortingOrder += 1;
        //}
    }
}
