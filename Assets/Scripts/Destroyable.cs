using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    public int wallHealth = 3;

    public void Damage(int damage)
    {
        GetComponent<AudioSource>().Play();
        wallHealth -= damage;
        if (wallHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
