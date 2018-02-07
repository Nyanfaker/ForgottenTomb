using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

    public Transform target;
    public float smoothSpeed;
    public string text;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.instance.playersTurn)
        {
            GameManager.instance.plaingGame = false;
            FindObjectOfType<CameraMove>().MoveCameraToTarget(target, smoothSpeed, text);
            target.GetComponent<Boss>().targetOn = true;
            Destroy(gameObject);
        }
    }
}
