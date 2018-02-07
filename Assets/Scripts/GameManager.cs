using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public bool plaingGame;

    public static GameManager instance = null;
    [HideInInspector]
    public bool playersTurn = true;
    [HideInInspector]
    public bool gameOver = false;
    public float turnDelay = 0.5f;
    public float restartLevelDelay = 1f;

    public List<Enemy> enemies;
    public List<Boss> bosses;
    public Text prefabMessage;
    public Animation gameOverAnimation;

    public bool inMenu;
    public bool isSpell;
    public static Vector2[] sides = new Vector2[] { Vector2.left, Vector2.up, Vector2.right, Vector2.down };

    bool enemiesMoving;
    Player player;
    Text playerStats;
    Text HP;
    Text MP;
    Slider HPBar;
    Slider MPBar;
    Slider EXPBar;
    CanvasGroup sandClock;

    Queue<Text> logQueue;
    Transform log;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        enemies = new List<Enemy>();
        bosses = new List<Boss>();
        logQueue = new Queue<Text>();
        
        InitGame();
    }

    private void Start()
    {
        log = GameObject.Find("LogPanel").GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<Player>();
        playerStats = GameObject.Find("Stats").GetComponent<Text>();
        HP = GameObject.Find("HP").GetComponent<Text>();
        MP = GameObject.Find("MP").GetComponent<Text>();
        HPBar = GameObject.Find("HPBar").GetComponent<Slider>();
        MPBar = GameObject.Find("MPBar").GetComponent<Slider>();
        EXPBar = GameObject.Find("EXPBar").GetComponent<Slider>();
        sandClock = GameObject.Find("SandClock").GetComponent<CanvasGroup>();
        RefreshStat();
        CreateLog();
    }

    void InitGame()
    {
        plaingGame = false;
        enemies.Clear();
        bosses.Clear();
    }

    void Update()
    {
        if (gameOver && playersTurn)
        {
            gameOverAnimation.Play();
            StartCoroutine(GameOver());
            return;
        }
        if (playersTurn || enemiesMoving || !plaingGame)
            return;
        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    public void RemoveEnemyToList(Enemy script)
    {
        enemies.Remove(script);
    }

    public void AddBossToList(Boss script)
    {
        bosses.Add(script);
    }

    public void RemoveBossToList(Boss script)
    {
        bosses.Remove(script);
    }

    IEnumerator MoveEnemies()
    {
        sandClock.alpha = 1; 
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].stunTime != 0)
            {
                enemies[i].stunTime -= 1;
                yield return null;
            }
            else
            {
                if (enemies[i].burnTime != 0)
                {
                    StartCoroutine(enemies[i].Burning());
                    yield return new WaitForSeconds(0.667f);
                    enemies[i].burnTime--;
                    enemies[i].DamageEnemy(2, enemies[i].onLeft, enemies[i].gameObject);
                    if (enemies[i].enabled == false)
                    {
                        RemoveEnemyToList(enemies[i]);
                        i--;
                        continue;
                    }
                    yield return new WaitForSeconds(0.5f);
                }
                enemies[i].MoveEnemy();
                if (enemies[i].isMove)
                {
                    yield return new WaitForSeconds(enemies[i].moveTime);
                }
                else
                {
                    yield return 0;
                }
            }
        }

        for (int i = 0; i < bosses.Count; i++)
        {
            if (bosses[i].targetOn)
            {
                if (bosses[i].burnTime != 0)
                {
                    bosses[i].Burning();
                    yield return new WaitForSeconds(0.5f);
                    bosses[i].TakeHit(2, bosses[i].gameObject);
                    if (bosses[i].enabled == false)
                    {
                        RemoveBossToList(bosses[i]);
                        i--;
                        continue;
                    }
                    yield return new WaitForSeconds(0.5f);
                }
                bosses[i].MoveBoss();
                
                if (bosses[i].burnTime != 0)
                {
                    bosses[i].burnTime--;
                }

                if (bosses[i].spellOn)
                {
                    yield return new WaitForSeconds(bosses[i].spellTime);
                }
                else
                {
                    yield return 0;
                }

                if (bosses[i] as BossDark)
                {
                    if (bosses[i].counterDark % 2 == 0 && bosses[i].counterDark != 6)
                    {
                        if (bosses[i].counterDark > 1 && bosses[i].counterDark != 4)
                        {
                            yield return new WaitForSeconds(1f);
                        }
                        i--;
                    }
                }
            }
        }

        sandClock.alpha = 0;
        playersTurn = true;
        enemiesMoving = false;
    }

    public void RefreshStat()
    {
        playerStats.text = "Health: " + player.currentHealth + "/" + player.health + "\n" +
            "Mana: " + player.currentMana + "/" + player.mana + "\n\n" +
            "Level: " + player.level + "\n" +
            "Exp: " + player.currentExp + "/" + player.needExp;
        HP.text = player.currentHealth + "/" + player.health;
        MP.text = player.currentMana + "/" + player.mana;
        HPBar.maxValue = player.health;
        HPBar.value = player.currentHealth;
        MPBar.maxValue = player.mana;
        MPBar.value = player.currentMana;
        EXPBar.maxValue = player.needExp;
        EXPBar.value = player.currentExp;
    }

    void CreateLog()
    {
        for (int i = 0; i < 6; i++)
        {
            Text message = Instantiate(prefabMessage, log);
            message.text = "";
            logQueue.Enqueue(message);
        }
    }

    public void Massage(GameObject thisObj, GameObject another, int damage)
    {
        Text message = Instantiate(prefabMessage, log);
        message.transform.SetAsFirstSibling();
        message.text = another.name + " deals " + damage + " damage to " + thisObj.name;
        logQueue.Enqueue(message);
        if (log.childCount > 6)
        {
            Destroy(logQueue.Dequeue().gameObject);
        }
    }

    public void Massage(Item item)
    {
        Text message = Instantiate(prefabMessage, log);
        message.transform.SetAsFirstSibling();
        message.text = "You picked up " + item.name + ".";
        logQueue.Enqueue(message);
        if (log.childCount > 6)
        {
            Destroy(logQueue.Dequeue().gameObject);
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(5.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
