using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sounds : MonoBehaviour {

    public Toggle toggle;
    AudioListener listener;

    private void Start()
    {
        listener = FindObjectOfType<Player>().GetComponent<AudioListener>();
    }

    public void CheckListener()
    {
        if (!AudioListener.pause)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    public void Sound()
    {
        if (toggle.isOn)
        {
            AudioListener.pause = false;
        }
        else
        {
            AudioListener.pause = true;
        }
    }
}
