using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDark : Boss
{

    public Pathfinding pathfinding;
    [HideInInspector]
    public GridPath gridPath;
    public bool invulnerable;
    

    protected override void Start()
    {
        gridPath = pathfinding.GetComponent<GridPath>();
        counterDark = -1;
        base.Start();
    }

    public override void TakeHit(int damage, GameObject enemy)
    {
        if (invulnerable)
        {
            FindObjectOfType<BlockPanel>().Texting("The sword passes through the dark without causing harm.");
            return;
        }
        base.TakeHit(damage, enemy);
    }

    public override void MoveBoss()
    {
        counterDark++;
        if (counterDark == 7)
        {
            counterDark = 0;
        }
        if (counterDark < 2 || counterDark == 4 || counterDark == 5)
        {
            return;
        }

        gridPath.CreateGrid();
        Vector2 dir = pathfinding.FindPath(transform.position, player.transform.position);
        int xDir = Mathf.RoundToInt(dir.x - transform.position.x);
        int yDir = Mathf.RoundToInt(dir.y - transform.position.y);
        if (xDir > 0)
        {
            bossAnimator.ResetTrigger("Left");
            bossAnimator.SetTrigger("Right");
        }
        else if (xDir < 0)
        {
            bossAnimator.ResetTrigger("Right");
            bossAnimator.SetTrigger("Left");
        }

        RaycastHit2D hit;
        bool canMove = Move(xDir, yDir, out hit);
        if (hit.transform == null)
            return;
        Player hitComponent = hit.transform.GetComponent<Player>();
        if (!canMove && hitComponent != null)
        {
            hitComponent.TakeHit(damage, true, gameObject);
        }
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
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

    private void OnMouseDown()
    {
        if (isBeside)
        {
            if (PowerKick.isSelect == true)
            {
                FindObjectOfType<BlockPanel>().Texting("The leg goes right through.");
                AfterCast();
            }
            else if (Ignite.isSelect == true)
            {
                FindObjectOfType<BlockPanel>().Texting("Fire can not be burned.");
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
}
