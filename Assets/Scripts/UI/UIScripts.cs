using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScripts : MonoBehaviour {

    public enum Windows { Log, Inventory, Spells, Menu, Help, Close }

    public static Windows window;
    [SerializeField]
    Windows thisWindow; 

    CanvasGroup log;
    CanvasGroup inv;
    CanvasGroup spl;
    CanvasGroup menu;
    CanvasGroup help;

    private void Start()
    {
        log = GameObject.Find("LogPanel").GetComponent<CanvasGroup>();
        inv = GameObject.Find("InvPanel").GetComponent<CanvasGroup>();
        spl = GameObject.Find("SpellPanel").GetComponent<CanvasGroup>();
        menu = GameObject.Find("MenuPanel").GetComponent<CanvasGroup>();
        help = GameObject.Find("HelpPanelInGame").GetComponent<CanvasGroup>();
        window = Windows.Close;
    }

    public void Button()
    {
        switch (window)
        {
            case Windows.Log:
                switch (thisWindow)
                {
                    case Windows.Log:
                        LogOff();
                        break;
                    case Windows.Inventory:
                        LogOff();
                        InvOn();
                        break;
                    case Windows.Spells:
                        LogOff();
                        SpellsOn();
                        break;
                    case Windows.Menu:
                        LogOff();
                        MenuOn();
                        break;
                }
                break;
            case Windows.Inventory:
                switch (thisWindow)
                {
                    case Windows.Log:
                        InvOff();
                        LogOn();
                        break;
                    case Windows.Inventory:
                        InvOff();
                        break;
                    case Windows.Spells:
                        InvOff();
                        SpellsOn();
                        break;
                    case Windows.Menu:
                        InvOff();
                        MenuOn();
                        break;
                }
                break;
            case Windows.Spells:
                switch (thisWindow)
                {
                    case Windows.Log:
                        SpellsOff();
                        LogOn();
                        break;
                    case Windows.Inventory:
                        SpellsOff();
                        InvOn();
                        break;
                    case Windows.Spells:
                        SpellsOff();
                        break;
                    case Windows.Menu:
                        SpellsOff();
                        MenuOn();
                        break;
                }
                break;
            case Windows.Menu:
                switch (thisWindow)
                {
                    case Windows.Log:
                        MenuOff();
                        LogOn();
                        break;
                    case Windows.Inventory:
                        MenuOff();
                        InvOn();
                        break;
                    case Windows.Spells:
                        MenuOff();
                        SpellsOn();
                        break;
                    case Windows.Menu:
                        MenuOff();
                        break;
                    case Windows.Help:
                        MenuOff();
                        HelpOn();
                        break;
                }
                break;
            case Windows.Help:
                switch (thisWindow)
                {
                    case Windows.Log:
                        HelpOff();
                        LogOn();
                        break;
                    case Windows.Inventory:
                        HelpOff();
                        InvOn();
                        break;
                    case Windows.Spells:
                        HelpOff();
                        SpellsOn();
                        break;
                    case Windows.Menu:
                        HelpOff();
                        MenuOn();
                        break;
                    case Windows.Help:
                        HelpOff();
                        break;
                }
                break;
            case Windows.Close:
                switch (thisWindow)
                {
                    case Windows.Log:
                        LogOn();
                        break;
                    case Windows.Inventory:
                        InvOn();
                        break;
                    case Windows.Spells:
                        SpellsOn();
                        break;
                    case Windows.Menu:
                        MenuOn();
                        break;
                }
                break;
        }
    }

    void LogOn()
    {
        GameManager.instance.inMenu = true;
        log.alpha = 1;
        window = Windows.Log;
    }

    void InvOn()
    {
        GameManager.instance.inMenu = true;
        inv.alpha = 1;
        inv.blocksRaycasts = true;
        inv.interactable = true;
        window = Windows.Inventory;
    }

    void SpellsOn()
    {
        GameManager.instance.inMenu = true;
        spl.alpha = 1;
        spl.blocksRaycasts = true;
        spl.interactable = true;
        window = Windows.Spells;
    }

    void MenuOn()
    {
        GameManager.instance.inMenu = true;
        GameObject.Find("SoundToggle").GetComponent<Sounds>().CheckListener();
        menu.alpha = 1;
        menu.blocksRaycasts = true;
        menu.interactable = true;
        window = Windows.Menu;
    }

    void HelpOn()
    {
        GameManager.instance.inMenu = true;
        help.alpha = 1;
        help.blocksRaycasts = true;
        help.interactable = true;
        window = Windows.Help;
    }

    void LogOff()
    {
        GameManager.instance.inMenu = false;
        log.alpha = 0;
        window = Windows.Close;
    }

    void InvOff()
    {
        GameManager.instance.inMenu = false;
        inv.alpha = 0;
        inv.blocksRaycasts = false;
        inv.interactable = false;
        window = Windows.Close;
    }

    void SpellsOff()
    {
        GameManager.instance.inMenu = false;
        spl.alpha = 0;
        spl.blocksRaycasts = false;
        spl.interactable = false;
        window = Windows.Close;
    }

    void MenuOff()
    {
        GameManager.instance.inMenu = false;
        menu.alpha = 0;
        menu.blocksRaycasts = false;
        menu.interactable = false;
        window = Windows.Close;
    }

    void HelpOff()
    {
        GameManager.instance.inMenu = false;
        help.alpha = 0;
        help.blocksRaycasts = false;
        help.interactable = false;
        window = Windows.Close;
    }
}
