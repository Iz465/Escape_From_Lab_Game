using UnityEngine;
using System.Collections;

public class HittablePillar : MonoBehaviour
{
    
    public IEnumerator DisablePillar(float time)
    {
      
        gameObject.SetActive(false);
        yield return new WaitForSeconds(time);
        gameObject.SetActive(true);
    }
}
