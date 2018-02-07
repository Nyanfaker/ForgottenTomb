using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BlockPanel : MonoBehaviour, IPointerClickHandler {

    Text textPanel;
    string allText;
    CanvasGroup cg;
    bool isTexting;
    Coroutine lastRoutine = null;
    bool m_LastSpeak;

    void Start () {
        cg = GetComponent<CanvasGroup>();
        textPanel = transform.GetChild(0).GetComponentInChildren<Text>();
        isTexting = false;
    }

    public void Texting(string text)
    {
        GameManager.instance.plaingGame = false;
        lastRoutine = StartCoroutine(AnimationText(text));
    }

    public void Texting(string text, bool lastSpeak)
    {
        GameManager.instance.plaingGame = false;
        lastRoutine = StartCoroutine(AnimationText(text));
        m_LastSpeak = lastSpeak;
    }

    IEnumerator AnimationText(string text)
    {
        BlockingPanel();

        isTexting = true;
        allText = text;
        int i = 0;
        textPanel.text = "   ";
        while (i < text.Length)
        {
            textPanel.text += text[i++];
            yield return new WaitForSeconds(0.06F);
        }
        allText = "";
        isTexting = false;
    }

    void BlockingPanel()
    {
        cg.alpha = cg.alpha == 0 ? 1 : 0;
        cg.blocksRaycasts = !cg.blocksRaycasts;
        cg.interactable = !cg.interactable;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTexting)
        {
            StopCoroutine(lastRoutine);
            textPanel.text = "   " + allText;
            allText = "";
            isTexting = false;
            return;
        }

        BlockingPanel();
        if (m_LastSpeak)
        {
            GameManager.instance.gameOver = true;
        }
        GameManager.instance.plaingGame = true;
    }
}
