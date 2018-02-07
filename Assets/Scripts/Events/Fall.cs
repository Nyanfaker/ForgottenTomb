using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour {

    public BossTroll target;
    public float smoothSpeed;
    public string text;

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameManager.instance.plaingGame = false;
        if (!GameManager.instance.playersTurn)
        {
            FindObjectOfType<CameraMove>().MoveCameraToTarget(target.transform, smoothSpeed, text);
            target.Fall();
            Destroy(gameObject);
        }
    }
}
