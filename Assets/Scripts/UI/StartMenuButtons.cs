using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StartMenuButtons : MonoBehaviour {

    string saveFile = "";

    public void ButtonNewGame()
    {
        StartCoroutine(NewGame());
    }

    IEnumerator NewGame()
    {
        GameObject.Find("DarkWindow").GetComponent<Animator>().SetTrigger("Click");
        yield return new WaitForSeconds(0.33f);
        GameObject.Find("StartMenuPanel").SetActive(false);
        GameObject.Find("HUDCanvas").GetComponent<Canvas>().enabled = true;
        GameManager.instance.plaingGame = true;
    }

    public void ContinueButton()
    {
        saveFile = Application.persistentDataPath + "/save.bin";
        if (File.Exists(saveFile))
        {
            StartCoroutine(Continue());
        }
    }

    IEnumerator Continue()
    {
        GameObject.Find("DarkWindow").GetComponent<Animator>().SetTrigger("Click");
        SaveLoadManager.instance.Load();
        yield return new WaitForSeconds(0.33f);
        GameObject.Find("StartMenuPanel").SetActive(false);
        GameObject.Find("HUDCanvas").GetComponent<Canvas>().enabled = true;
        GameManager.instance.plaingGame = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
