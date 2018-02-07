using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : InteractableObj {

    public Transform target;
    public Boss boss;
    public bool haveBoss;

    public override void Interacte()
    {
        StartCoroutine(Teleportation());
    }

    IEnumerator Teleportation()
    {
        GameManager.instance.plaingGame = false;
        if (haveBoss)
        {
            boss.targetOn = false;
        }
        GameObject.Find("DarkWindow").GetComponent<Animator>().SetTrigger("Click");
        SaveLoadManager.instance.level += 1;
        SaveLoadManager.instance.Save();
        yield return new WaitForSeconds(0.33f);
        GameObject.Find("Player").transform.position = target.position;
        GameManager.instance.plaingGame = true;
    }
}
