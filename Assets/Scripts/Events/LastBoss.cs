using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBoss : MonoBehaviour {

    public Transform target;
    public string textWithSword;
    public string textWithPiece;
    public string textWithout;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.instance.playersTurn)
        {
            GameManager.instance.plaingGame = false;
            if (InventoryController.instance.CheckItem(9))
            {
                FindObjectOfType<CameraMove>().MoveCameraToTarget(target, 0.125f, textWithSword);
            }
            else if (InventoryController.instance.CheckItem(6) ||
                     InventoryController.instance.CheckItem(7) ||
                     InventoryController.instance.CheckItem(8))
            {
                FindObjectOfType<CameraMove>().MoveCameraToTarget(target, 0.125f, textWithPiece);
            }
            else
            {
                FindObjectOfType<CameraMove>().MoveCameraToTarget(target, 0.125f, textWithout);
            }
            target.GetComponent<Boss>().targetOn = true;
            Destroy(gameObject);
        }
    }
}
