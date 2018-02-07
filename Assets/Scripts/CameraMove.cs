using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public GameObject player;

    BlockPanel panel;

    Vector3 offset = new Vector3(0f, 0f, -10f);
    Vector3 menu = new Vector3(0, -20, -10);

    private void Start()
    {
        panel = FindObjectOfType<BlockPanel>();
        transform.position = menu;
    }

    void FixedUpdate() {
        if (GameManager.instance.plaingGame)
        {
            transform.position = player.transform.position + offset;
        }
    }

    public void MoveCameraToTarget(Transform target, float smoothSpeed, string text)
    {
        StartCoroutine(SmoothMoving(target, smoothSpeed, text));
    }

    IEnumerator SmoothMoving(Transform target, float smoothSpeed, string text)
    {
        Vector3 targWithOffset = target.position + offset;
        float dis = (transform.position - targWithOffset).sqrMagnitude;
        while (dis >= 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targWithOffset, smoothSpeed);
            dis = (transform.position - targWithOffset).sqrMagnitude;
            yield return new WaitForSeconds(0.01f);
        }
        panel.Texting(text);
    }
}
