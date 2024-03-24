using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera myCam;
    public float one = 1;
    public float two = 2;

    public void Start()
    {
        myCam = Camera.main;
        StartCoroutine(Shake(one, two));
    }

    public IEnumerator Shake(float duration, float magnitud)
    {
        Vector3 oriPosition = myCam.transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitud;
            float y = Random.Range(-1, 1) * magnitud;

            myCam.transform.localPosition = new Vector3(x, y, oriPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        myCam.transform.localPosition = oriPosition;
    }
}
