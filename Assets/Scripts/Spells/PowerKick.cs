using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerKick : MonoBehaviour {

    public int manaCost;
    public GameObject greenTile;
    public LayerMask blockingLayer;
    BoxCollider2D playerCollider;
    Player player;
    List<Enemy> enemies;
    List<Boss> bosses;
    List<GameObject> tiles;

    CanvasGroup panel;
    CanvasGroup cancel;

    static public bool isSelect;


    private void Start()
    {
        panel = GameObject.Find("ButtonPanel").GetComponent<CanvasGroup>();
        cancel = GameObject.Find("Cancel").GetComponent<CanvasGroup>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerCollider = player.GetComponent<BoxCollider2D>();
        enemies = new List<Enemy>();
        bosses = new List<Boss>();
        tiles = new List<GameObject>();
    }

    public void EnemyScaner()
    {
        if (player.currentMana < manaCost || !GameManager.instance.playersTurn)
        {
            //NOT ENOUGH MANA
            return;
        }
        panel.interactable = false;
        playerCollider.enabled = false;
        for (int i = 0; i < 4; i++)
        {
            Vector2 start = player.transform.position;
            Vector2 end = start + GameManager.sides[i];
            RaycastHit2D hit;
            hit = Physics2D.Linecast(start, end, blockingLayer);
            if (hit && hit.transform.tag == "Enemy")
            {
                GameObject tile = Instantiate(greenTile, hit.transform.position, Quaternion.identity);
                tiles.Add(tile);
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                enemy.isBeside = true;
                enemies.Add(enemy);
            }
            else if(hit && hit.transform.tag == "Boss")
            {
                Boss boss = hit.transform.GetComponent<Boss>();
                if (boss.oneTile)
                {
                    GameObject tile = Instantiate(greenTile, hit.transform.position, Quaternion.identity);
                    tiles.Add(tile);
                    boss.isBeside = true;
                    bosses.Add(boss);
                }
                else
                {
                    GameObject tile = Instantiate(greenTile, hit.transform.position, Quaternion.identity);
                    tile.transform.localScale = new Vector3(2, 2, 1);
                    tiles.Add(tile);
                    boss.isBeside = true;
                    bosses.Add(boss);
                }
            }
        }
        playerCollider.enabled = true;
        GameManager.instance.isSpell = true;
        isSelect = true;

        cancel.alpha = 1;
        cancel.blocksRaycasts = true;
        cancel.interactable = true;
    }

    public void Cancel()
    {
        if (isSelect)
        {
            foreach (GameObject tile in tiles)
            {
                Destroy(tile);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.isBeside = false;
            }
            foreach (Boss boss in bosses)
            {
                boss.isBeside = false;
            }
            tiles.Clear();
            enemies.Clear();
            bosses.Clear();
            GameManager.instance.isSpell = false;
            GameManager.instance.inMenu = false;

            isSelect = false;

            cancel.alpha = 0;
            cancel.blocksRaycasts = false;
            cancel.interactable = false;

            panel.interactable = true;
        }
    }
}
