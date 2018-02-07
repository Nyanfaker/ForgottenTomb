using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ignite : MonoBehaviour {

    public int manaCost;
    public GameObject greenTile;
    public LayerMask blockingLayer;
    BoxCollider2D playerCollider;
    Player player;
    List<Enemy> enemies;
    List<Enemy> hitsEnemies;
    List<Boss> bosses;
    Boss currentBoss;
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
        enemies = GameManager.instance.enemies;
        bosses = GameManager.instance.bosses;
        hitsEnemies = new List<Enemy>();
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
        RaycastHit2D hit;
        for (int i = 0; i < enemies.Count; i++)
        {
            hit = Physics2D.Linecast(player.transform.position, enemies[i].transform.position, blockingLayer);
            if (hit && hit.transform.tag == "Enemy")
            {
                GameObject tile = Instantiate(greenTile, hit.transform.position, Quaternion.identity);
                tiles.Add(tile);
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                enemy.isBeside = true;
                hitsEnemies.Add(enemy);
            }
        }
        for (int i = 0; i < bosses.Count; i++)
        {
            hit = Physics2D.Linecast(player.transform.position, bosses[i].transform.position, blockingLayer);
            if (hit && hit.transform.tag == "Boss")
            {
                Boss boss = hit.transform.GetComponent<Boss>();
                if (boss.oneTile)
                {
                    GameObject tile = Instantiate(greenTile, hit.transform.position, Quaternion.identity);
                    tiles.Add(tile);
                    boss.isBeside = true;
                    currentBoss = boss;
                }
                else
                {
                    GameObject tile = Instantiate(greenTile, hit.transform.position, Quaternion.identity);
                    tile.transform.localScale = new Vector3(2, 2, 1);
                    tiles.Add(tile);
                    boss.isBeside = true;
                    currentBoss = boss;
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
            foreach (Enemy enemy in hitsEnemies)
            {
                enemy.isBeside = false;
            }
            if (currentBoss != null)
            {
                currentBoss.isBeside = false;
            }
            tiles.Clear();
            hitsEnemies.Clear();

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
