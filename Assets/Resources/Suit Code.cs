using UnityEngine;
using System.Collections;

public class SuitCode : MonoBehaviour
{
   
    [SerializeField] private AnimationClip walk;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Transform targetPosition;

    
    private void Start()
    {
        transform.position = new Vector3(-13.82f, 0.849f, 4.34f);
    }

    void StartWalk()
    {
        StartCoroutine(MoveForward(walk.length));
    }

    private IEnumerator MoveForward(float clipLength)
    {
        Debug.Log("STARTING WALK");
        float time = 0;
        Vector3 start = transform.position;
        Vector3 end = transform.position + transform.forward * 2.5f;

        while (time < clipLength)
        {
            transform.position = Vector3.Lerp(start, end, time / clipLength);
            time += Time.deltaTime;
            yield return null;

        }

        transform.position = end;
    }

  

}
