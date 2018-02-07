using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Moving {

    public int damage;
    public int enemyHealth;
    public int exp;
    public int skipMove;
    public float distance;
    public AudioSource audioDeathSource;
    public AudioClip audioDeath;

    public bool havePath;
    public Pathfinding pathfinding;
    [HideInInspector]
    public GridPath gridPath;
    Animator animator;
    Transform target;
    Player player;
    public bool onLeft;
    int counter;
    [HideInInspector]
    public bool isBeside;
    [HideInInspector]
    public bool isMove;

    public int stunTime;
    public int burnTime;
    SpriteRenderer sr;

    private void Update()
    {
        sps.SetPosition();
    }

    protected override void Start()
    {
        if (havePath)
        {
            gridPath = pathfinding.GetComponent<GridPath>();
        }
        counter = 1;
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponentInChildren<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.GetComponent<Player>();
        sr = GetComponentInChildren<SpriteRenderer>();
        onLeft = true;
        base.Start();
    }

    public void MoveEnemy()
    {
        Vector2 dis = target.position - transform.position;
        isMove = true;
        if (dis.sqrMagnitude > distance * distance)
        {
            isMove = false;
            return;
        }
        gridPath.CreateGrid();
        Vector2 dir = pathfinding.FindPath(transform.position, target.position);
        int xDir = Mathf.RoundToInt(dir.x - transform.position.x);
        int yDir = Mathf.RoundToInt(dir.y - transform.position.y);
        if (xDir > 0)
        {
            onLeft = false;
            animator.ResetTrigger("Left");
            animator.SetTrigger("Right");
            animator.SetBool("LeftBool", false);
        }
        else if (xDir < 0)
        {
            onLeft = true;
            animator.ResetTrigger("Right");
            animator.SetTrigger("Left");
            animator.SetBool("LeftBool", true);
        }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (counter != skipMove)
        {
            counter++;
            return;
        }
        base.AttemptMove<T>(xDir, yDir);
        counter = 1;
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        hitPlayer.TakeHit(damage, onLeft, gameObject);
        if (onLeft)
        {
            animator.SetTrigger("AttL");
        } else
        {
            animator.SetTrigger("AttR");
        }
    }

    public void DamageEnemy(int damage, bool isLeft, GameObject enemy)
    {
        if (isLeft)
        {
            animator.SetBool("LeftBool", true);
            animator.SetTrigger("HitRight");
        }
        else
        {
            animator.SetBool("LeftBool", false);
            animator.SetTrigger("HitLeft");
        }
        enemyHealth -= damage;
        GameManager.instance.Massage(gameObject, enemy, damage);
        if (enemyHealth <= 0)
        {
            AudioSource aus;
            aus = Instantiate(audioDeathSource);
            aus.clip = audioDeath;
            aus.Play();
            player.TakeExp(exp);
            if (GameManager.instance.playersTurn)
            {
                GameManager.instance.RemoveEnemyToList(this);
            }
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (isBeside)
        {
            if (PowerKick.isSelect == true)
            {
                Vector2 dis = transform.position - target.position;
                int xDir = 0;
                int yDir = 0;
                for (int i = 0; i < GameManager.sides.Length; i++)
                {
                    if (dis == GameManager.sides[i])
                    {
                        xDir = (int)dis.x;
                        yDir = (int)dis.y;
                    }
                }

                RaycastHit2D hit;
                hit = Hit(xDir * 2, yDir * 2, m_blockingLayer);
                if (!hit)
                {
                    Vector2 end = (Vector2)transform.position + new Vector2(xDir * 2, yDir * 2);
                    StartCoroutine(SmoothMovement(end));
                    AfterCast();
                    return;
                }
                else
                {
                    if (Mathf.Abs(hit.distance) > 1)
                    {
                        Vector2 end = (Vector2)transform.position + new Vector2(xDir, yDir);
                        StartCoroutine(SmoothMovement(end));
                        stunTime += 2;
                        AfterCast();
                        return;
                    }
                    else
                    {
                        stunTime += 3;
                        AfterCast();
                        return;
                    }
                }

            }
            else if (Ignite.isSelect == true)
            {
                burnTime += 3;
                StartCoroutine(Burning());
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

    public IEnumerator Burning()
    {
        Color offset = sr.color;
        Color red = new Color(1f, 0.4f, 0.4f);
        float elapsedTime = 0.0f;
        float totalTime = 0.2f;
        while (sr.color != red)
        {
            elapsedTime += Time.deltaTime;
            sr.color = Color.Lerp(offset, red, (elapsedTime / totalTime));
            yield return null;
        }
        elapsedTime = 0.0f;
        while (sr.color != offset)
        {
            elapsedTime += Time.deltaTime;
            sr.color = Color.Lerp(red, offset, (elapsedTime / totalTime));
            yield return null;
        }
        if (GameManager.instance.playersTurn)
        {
            AfterCast();
        }
    }
}
