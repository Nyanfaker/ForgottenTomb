using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningTorch : InteractableObj {

    BossDark boss;
    public GameObject torch;
    SpriteMask sm;
    Transform halo;
    Vector3 delta = new Vector3(0.075f, 0.075f, 0);

    private void Start()
    {
        sm = FindObjectOfType<SpriteMask>();
        halo = GameObject.Find("Halo").GetComponent<Transform>();
        boss = GameObject.Find("Dark").GetComponent<BossDark>();
    }

    public override void Interacte()
    {
        Instantiate(torch, transform.position, Quaternion.identity, transform.parent);
        sm.alphaCutoff -= 0.075f;
        halo.localScale += delta;
        boss.invulnerable = false;
        boss.TakeHit(1, gameObject);
        boss.invulnerable = true;
        GameManager.instance.playersTurn = false;
        Destroy(gameObject);
    }
}
