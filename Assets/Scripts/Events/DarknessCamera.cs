using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessCamera : MonoBehaviour {

    bool work;
    SpriteMask sm;
    Transform halo;
    Vector3 target = new Vector3(0.22f, 0.22f, 1);

    private void Start()
    {
        sm = FindObjectOfType<SpriteMask>();
        halo = GameObject.Find("Halo").GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        work = true;
    }

    void Update () {
        if (work)
        {
            sm.alphaCutoff = Mathf.Lerp(sm.alphaCutoff, 0.8f, Time.deltaTime);
            halo.localScale = Vector3.Lerp(halo.localScale, target, Time.deltaTime);
            float delta = 0.8f - sm.alphaCutoff;
            if (delta < 0.01f)
            {
                sm.alphaCutoff = 0.8f;
                halo.localScale = target;
                Destroy(gameObject);
            }
        }
	}
}
