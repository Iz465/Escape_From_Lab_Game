using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
 
    
    // Camera shake helps add an oomph to the attacks to make the combat more satisfying
    // The camera shakes by being moved every frame over a random max and min value the player gives in a certain time.
    public IEnumerator Shake(float xValue, float yValue, float timer)
    {

        float time = 0;
        Vector3 startLoc = transform.position;

       
        while (time < timer)
        {
            float x = Random.Range(.2f, .3f);
            float y = Random.Range(.2f, .3f);

            transform.position = startLoc + new Vector3(x, y, 0.08f);

            //    transform.position = startLoc + new Vector3(x, y, 0f);


            time += Time.deltaTime;
            yield return null;
        }

        transform.position = startLoc; 

    }
}
