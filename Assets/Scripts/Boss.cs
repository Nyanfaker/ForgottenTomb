using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public int waitingForMove;
    public float spellTime;
    public bool targetOn;
    public int damage;
    public int bossHealth;
    public int exp;
    public bool oneTile;
    public LayerMask blockingLayer;
    public string dyingText;
    public Item item;
    public AudioSource audioSource;
    public AudioClip audioDeath;

    [HideInInspector]
    public bool spellOn;
    [HideInInspector]
    public bool isBeside;
    [HideInInspector]
    public int burnTime;
    [HideInInspector]
    public bool isDying;
    [HideInInspector]
    public int counterDark;

    protected Player player;
    protected BoxCollider2D boxCollider;
    protected Rigidbody2D rb2D;
    protected SpriteRenderer sr;
    public Animator bossAnimator;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<Player>();
        GameManager.instance.AddBossToList(this);
    }

    public virtual void MoveBoss() { }

    public virtual void TakeHit(int damage, GameObject enemy)
    {
        if (isDying)
        {
            return;
        }
        bossHealth -= damage;
        GameManager.instance.Massage(gameObject, enemy, damage);
        if (bossHealth <= 0)
        {
            player.TakeExp(exp);
            if (item != null)
            {
                InventoryController.instance.AddNewItem(item);
            }
            StartCoroutine(Dying());
        }
    }

    public virtual void Burning() { }

    public virtual IEnumerator Dying()
    {
        GameManager.instance.plaingGame = false;
        isDying = true;
        yield return new WaitForSeconds(0.4f);
        if (gameObject.name != "Beholder")
        {
            FindObjectOfType<CameraMove>().MoveCameraToTarget(transform, 0.125f, dyingText + "\n\n   *You picked up something interesting*");
        } else
        {
            FindObjectOfType<CameraMove>().MoveCameraToTarget(transform, 0.125f, dyingText);
        }
        yield return new WaitForSeconds(0.4f);
        bossAnimator.SetTrigger("Dying");
        AudioSource aus;
        aus = Instantiate(audioSource);
        aus.clip = audioDeath;
        aus.Play();
        yield return new WaitForSeconds(2f);
        GameManager.instance.RemoveBossToList(this);
        Destroy(gameObject);
    }
}
