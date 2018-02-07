using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speakers : InteractableObj {

    public bool lastNPS;
    public string text;
    BlockPanel blockPanel;

    private void Start()
    {
        blockPanel = GameObject.Find("BlockPanel").GetComponent<BlockPanel>();
    }

    public override void Interacte()
    {
        if (!lastNPS)
        {
            blockPanel.Texting(text);
        } else
        {
            blockPanel.Texting(text, true);
        }
    }
}
