using System.Collections;
using UnityEngine;

public class GoToObject : MonoBehaviour
{
    [SerializeField] private Transform destination;
 

   

    public void AllowTravel()
    {
        StartCoroutine(StartTravel(2));
    }

    private IEnumerator StartTravel(float timer)
    {
        float time = 0;

        Vector3 original = transform.position;
        Vector3 end = destination.position;

        while (time < timer)
        {
            transform.position = Vector3.Lerp(original, end, time / timer);
            time += Time.deltaTime;
            yield return null;
        }
    }
   
    
}
