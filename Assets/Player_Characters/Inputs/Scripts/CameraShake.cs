using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float timer)
    {

        float time = 0;
        Vector3 startLoc = gameObject.transform.position;
       
        while (time < timer)
        {
            float x = Random.Range(-0.30f, 0.30f);
            float y = Random.Range(-0.30f, 0.30f);

            transform.position = startLoc + new Vector3(x, y, 0f);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = startLoc;

    }
}
