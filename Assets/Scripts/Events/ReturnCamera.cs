using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnCamera : MonoBehaviour {

    SpriteMask sm;
    Transform halo;
    Vector3 target = new Vector3(0.51f, 0.51f, 1);

    private void Start()
    {
        sm = FindObjectOfType<SpriteMask>();
        halo = GameObject.Find("Halo").GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sm.alphaCutoff = 0.5f;
        halo.localScale = target;
        Destroy(gameObject);
    }
}
