using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private static Transform tr;
    private static float elapsed, i_Duration, i_Power, percentComplete;
    private static Vector3 originalPos;

    void Start()
    {
        percentComplete = 1;
        tr = GetComponent<Transform>();
    }

    public static void Shake(float duration, float power)
    {
        if (percentComplete == 1) originalPos = tr.localPosition;
        elapsed = 0;
        i_Duration = duration;
        i_Power = power;
    }

    void Update()
    {
        if (elapsed < i_Duration)
        {
            elapsed += Time.deltaTime;
            percentComplete = elapsed / i_Duration;
            percentComplete = Mathf.Clamp01(percentComplete);
            Vector3 rnd = Random.insideUnitSphere * i_Power * (1f - percentComplete);
            tr.localPosition = originalPos + new Vector3(rnd.x, rnd.y, 0);
        }
    }
}
