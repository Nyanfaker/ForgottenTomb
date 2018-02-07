using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeholder : Boss {

    Vector3 persPos;
    public GameObject persona;
    int currentNumber = 0;
    public LineRenderer lr;
    public AudioClip beam;

    public override void MoveBoss()
    {
        if (isDying)
        {
            return;
        }
        if (currentNumber != waitingForMove)
        {
            spellOn = false;
            bossAnimator.SetTrigger("Cast");
            currentNumber++;
            return;
        }
        spellOn = true;
        currentNumber = 0;
        lr.SetPosition(1, player.transform.position);
        bossAnimator.SetTrigger("Beam");
        AudioSource aus;
        aus = Instantiate(audioSource);
        aus.clip = beam;
        aus.Play();
        StartCoroutine(Beam());
    }

    IEnumerator Beam()
    {
        yield return new WaitForSeconds(1.12f);
        player.TakeHit(damage, true, gameObject);
    }

    private void OnMouseDown()
    {
        if (isBeside)
        {
            if (PowerKick.isSelect == true)
            {
                FindObjectOfType<BlockPanel>().Texting("I can't. He hovers.");
                AfterCast();
            }
            else if (Ignite.isSelect == true)
            {
                FindObjectOfType<BlockPanel>().Texting("His psi-forces do not allow this.");
                AfterCast();
            }
        }
    }

    public void AfterCast()
    {
        if (PowerKick.isSelect == true)
        {
            PowerKick kickSpell = GameObject.Find("Power Kick").GetComponent<PowerKick>();
            kickSpell.Cancel();
        }
        else if (Ignite.isSelect == true)
        {
            Ignite ignite = GameObject.Find("Ignite").GetComponent<Ignite>();
            ignite.Cancel();
        }
        GameManager.instance.playersTurn = false;
    }

    void CreatePersona()
    {
        Vector3 player = FindObjectOfType<Player>().transform.position;
        persPos = player + new Vector3(2, 0, 0);
        Instantiate(persona, persPos, Quaternion.identity, transform.parent);
    }
}
