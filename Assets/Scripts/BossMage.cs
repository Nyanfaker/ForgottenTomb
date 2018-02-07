using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMage : Boss {

    public Enemy zombie;
    public Transform[] spawns;
    public GameObject spell;
    public Transform enemiesParent;
    public AudioClip magicAudio;

    public override void MoveBoss()
    {
        
        if (burnTime == 0 && !isDying)
        {
            spellOn = true;
            StartCoroutine(CreateZombie());
        } else
        {
            spellOn = false;
            return;
        }
    }

    IEnumerator CreateZombie()
    {
        bossAnimator.SetTrigger("Cast");
        AudioSource aus;
        aus = Instantiate(audioSource);
        aus.clip = magicAudio;
        aus.Play();
        yield return new WaitForSeconds(spellTime - 0.7f);
        for (int i = 0; i < spawns.Length; i++)
        {
            RaycastHit2D hit;
            Enemy currentZombie;
            hit = Physics2D.CircleCast(spawns[i].position, 0.1f, Vector2.left, 0.1f ,blockingLayer);
            if (hit)
            {
                continue;
            }
            currentZombie = Instantiate(zombie, spawns[i].position, Quaternion.identity, enemiesParent);
            currentZombie.pathfinding = GameObject.Find("APath_3").GetComponent<Pathfinding>();
            currentZombie.gridPath = GameObject.Find("APath_3").GetComponent<GridPath>();
            currentZombie.GetComponentInChildren<Animation>().Play();
            yield return new WaitForSeconds(0.7f);
            currentZombie.GetComponentInChildren<Animator>().enabled = true;
            break;
        }
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
                }
                else
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
                Vector2 end = start + new Vector2(xDir, yDir);
                boxCollider.enabled = false;
                RaycastHit2D hit;
                hit = Physics2D.Linecast(start, end, blockingLayer);
                boxCollider.enabled = true;
                if (!hit)
                {
                    StartCoroutine(SmoothMovement(end));
                    AfterCast();
                    return;
                }
                else
                {
                    if (Mathf.Abs(hit.distance) > 1)
                    {
                        end = (Vector2)transform.position + new Vector2(xDir / 2, yDir / 2);
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
}
