using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orgiPos = transform.localPosition;

        float time = 0;

        while(time < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(orgiPos.x + x, orgiPos.y + y, orgiPos.z);

            time += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = orgiPos;
    }

    public void StartShake(float duration)
    {
        StartCoroutine(Shake(duration, 1));
    }
}
