using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Moving
{
    enum Attack { AttR, AttB, AttL, AttU }

    public int health = 10;
    public int mana = 5;
    public int level = 1;
    public int needExp = 10;
    [HideInInspector]
    public int currentExp;
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int currentMana;
    public int damage = 1;
    public LayerMask interactable;

    Attack attack;
    Animator animator;
    bool onRight;
    RaycastHit2D hit;

#if !UNITY_STANDALONE
    protected Vector2 startingTouch;
    protected bool isSwiping = false;
#endif

    protected override void Start()
    {
        currentHealth = health;
        currentMana = mana;
        currentExp = 0;
        animator = GetComponentInChildren<Animator>();
        base.Start();
    }

    private void Update()
    {
        sps.SetPosition();
        weaponSPS.SetPosition();
        if (!GameManager.instance.playersTurn || GameManager.instance.isSpell || !GameManager.instance.plaingGame) return;

        int horizontal = 0;
        int vertical = 0;

#if UNITY_EDITOR || UNITY_STANDALONE
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

#else
        if (Input.touchCount == 1 && !GameManager.instance.inMenu)
        {
			if(isSwiping)
			{
				Vector2 diff = Input.GetTouch(0).position - startingTouch;
				diff = new Vector2(diff.x/Screen.width, diff.y/Screen.width);
				if(diff.magnitude > 0.2f) 
				{
					if(Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
					{
						if(diff.y < 0)
						{
                            vertical = -1;
						}
						else
						{
                            vertical = 1;
						}
					}
					else
					{
						if(diff.x < 0)
						{
							horizontal = -1;
						}
						else
						{
                            horizontal = 1;
						}
					}
					isSwiping = false;
				}
            }

			if(Input.GetTouch(0).phase == TouchPhase.Began)
			{
				startingTouch = Input.GetTouch(0).position;
				isSwiping = true;
			}
			else if(Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				isSwiping = false;
			}
        } else {
        isSwiping = false;
        }

#endif

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            hit = Hit(horizontal, vertical, interactable);
            if (hit && hit.transform.tag == "Interactable")
            {
                hit.transform.GetComponent<InteractableObj>().Interacte();
                return;
            }
            hit = Hit(horizontal, vertical, m_blockingLayer);
            if (hit.transform == null || hit.transform.tag == "Enemy")
            {
                AttemptMove<Enemy>(horizontal, vertical);
            }
            else if (hit.transform.tag == "Destroyable")
            {
                AttemptMove<Destroyable>(horizontal, vertical);
            }
            else if (hit.transform.tag == "Boss")
            {
                AttemptMove<Boss>(horizontal, vertical);
            }
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        int side = xDir * 2 + yDir;
        switch (side)
        {
            case 2:
                attack = Attack.AttR;
                animator.ResetTrigger("Left");
                animator.SetTrigger("Right");
                animator.SetBool("RightBool", true);
                onRight = true;
                break;
            case -1:
                attack = Attack.AttB;
                break;
            case -2:
                attack = Attack.AttL;
                animator.ResetTrigger("Right");
                animator.SetTrigger("Left");
                animator.SetBool("RightBool", false);
                onRight = false;
                break;
            case 1:
                attack = Attack.AttU;
                break;
            default:
                break;
        }
        base.AttemptMove<T>(xDir, yDir);
        GameManager.instance.playersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        switch (attack)
        {
            case Attack.AttL:
                animator.SetTrigger("AttL");
                break;
            case Attack.AttB:
                animator.SetTrigger("AttB");
                break;
            case Attack.AttR:
                animator.SetTrigger("AttR");
                break;
            case Attack.AttU:
                animator.SetTrigger("AttU");
                break;
            default:
                break;
        }
        if (component is Enemy)
        {
            Enemy hitEnemy = component as Enemy;
            hitEnemy.DamageEnemy(damage, onRight, gameObject);
        }
        else if (component is Destroyable)
        {
            Destroyable hitEnemy = component as Destroyable;
            hitEnemy.Damage(1);
        }
        else if (component is Boss)
        {
            Boss hitEnemy = component as Boss;
            hitEnemy.TakeHit(damage, gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.tag == "Object")
       {
            ItemTile itemTile = other.GetComponent<ItemTile>();
            InventoryController.instance.AddNewItem(itemTile.item);
            GameManager.instance.Massage(itemTile.item);
            Destroy(other.gameObject);
       }
    }

    public void TakeHit(int loss, bool isRight, GameObject enemy)
    {
        if (isRight)
        {
            animator.SetTrigger("HitOnRight");
            animator.SetBool("RightBool", true);
        }
        else
        {
            animator.SetTrigger("HitOnLeft");
            animator.SetBool("RightBool", false);
        }
        currentHealth -= loss;
        GameManager.instance.Massage(gameObject, enemy, loss);
        GameManager.instance.RefreshStat();
        CheckIfGameOver();
    }

    public void TakeExp(int exp)
    {
        currentExp += exp;
        GameManager.instance.RefreshStat();
        if (currentExp >= needExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        while (currentExp >= needExp)
        {
            level += 1;
            health += 2;
            mana += 1;
            int remainder = currentExp - needExp;
            needExp += 5;
            currentExp = remainder;
        } 
        currentHealth = health;
        currentMana = mana;
        GameManager.instance.RefreshStat();
    }

    private void CheckIfGameOver()
    {
        if (currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        GameManager.instance.plaingGame = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        GameObject.Find("DarkWindow").GetComponent<Animator>().SetTrigger("Click");
        yield return new WaitForSeconds(0.33f);
        SceneManager.LoadScene(0);
    }
}