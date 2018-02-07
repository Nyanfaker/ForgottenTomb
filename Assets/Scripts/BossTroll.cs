using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTroll : Boss {

    public GameObject spell;
    int currentNumber = 0;
    public AudioClip audioFall;
    public AudioClip smash;
    

    public override void MoveBoss()
    {
        if (isDying)
        {
            return;
        }
        if (currentNumber != waitingForMove)
        {
            spellOn = false;
            spell.SetActive(true);
            currentNumber++;
            return;
        }
        spellOn = true;
        currentNumber = 0;
        spell.SetActive(false);
        bossAnimator.SetBool("Smash", true);
        StartCoroutine(Smash());
    }

    IEnumerator Smash()
    {
        yield return new WaitForSeconds(spellTime - 0.2f);
        AudioSource aus;
        aus = Instantiate(audioSource);
        aus.clip = smash;
        aus.Play();
        CameraShake.Shake(1, 1);
        yield return new WaitForSeconds(0.2f);
        Vector2 dis = transform.position - player.transform.position;
        if (dis.magnitude < 2)
        {
            player.TakeHit(damage * 3, true, gameObject);
        }
        else
        {
            player.TakeHit(damage, true, gameObject);
        }
        bossAnimator.SetBool("Smash", false);
    }

    private void OnMouseDown()
    {
        if (isBeside)
        {
            if (PowerKick.isSelect == true)
            {
                Vector2 dis = transform.position - player.transform.position;
                float xDir = 0;
                float yDir = 0;
                if (Mathf.Abs(dis.x) > Mathf.Abs(dis.y))
                {
                    dis.y = 0;
                    if (dis.x > 0)
                    {
                        xDir = 2;
                    }
                    else
                    {
                        xDir = -2;
                    }
                } else
                {
                    dis.x = 0;
                    if (dis.y > 0)
                    {
                        yDir = 2;
                    }
                    else
                    {
                        yDir = -2;
                    }
                }
                
                Vector2 start = transform.position;
                Vector2 dir = new Vector2(xDir, yDir);
                boxCollider.enabled = false;
                RaycastHit2D hit;
                hit = Physics2D.BoxCast(start, Vector2.one, 0 , dir, Mathf.Infinity, blockingLayer);
                boxCollider.enabled = true;
                if (!hit)
                {
                    Vector2 end = (Vector2)transform.position + new Vector2(xDir, yDir);
                    StartCoroutine(SmoothMovement(end));
                    AfterCast();
                    return;
                }
                else
                {
                    if (Mathf.Abs(hit.distance) > 1)
                    {
                        Vector2 end = (Vector2)transform.position + new Vector2(xDir / 2, yDir / 2);
                        StartCoroutine(SmoothMovement(end));
                        AfterCast();
                        return;
                    }
                    else
                    {
                        AfterCast();
                        return;
                    }
                }

            }
            else if (Ignite.isSelect == true)
            {
                burnTime += 3;
                Burning();
                return;
            }
        }
    }

    public void AfterCast()
    {
        if (PowerKick.isSelect == true)
        {
            PowerKick kickSpell = GameObject.Find("Power Kick").GetComponent<PowerKick>();
            player.currentMana -= kickSpell.manaCost;
            kickSpell.Cancel();
        }
        else if (Ignite.isSelect == true)
        {
            Ignite ignite = GameObject.Find("Ignite").GetComponent<Ignite>();
            player.currentMana -= ignite.manaCost;
            ignite.Cancel();
        }
        GameManager.instance.RefreshStat();
        GameManager.instance.playersTurn = false;
    }

    public override void Burning()
    {
        StartCoroutine(BurningCour());
    }

    IEnumerator BurningCour()
    {
        bossAnimator.SetTrigger("Burn");
        yield return new WaitForSeconds(0.667f);
        if (GameManager.instance.playersTurn)
        {
            AfterCast();
        }
    }

    IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, 5 * Time.deltaTime);
            rb2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

    }

    public void Fall()
    {
        StartCoroutine(Falling());
    }

    IEnumerator Falling()
    {
        isDying = true;
        bossAnimator.SetTrigger("Fall");
        AudioSource aus;
        aus = Instantiate(audioSource);
        aus.clip = audioFall;
        aus.Play();
        yield return new WaitForSeconds(1);
        GameManager.instance.RemoveBossToList(this);
        Destroy(gameObject);
    }
}

