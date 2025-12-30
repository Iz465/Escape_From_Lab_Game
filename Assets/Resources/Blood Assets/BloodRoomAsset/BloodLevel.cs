using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BloodLevel : MonoBehaviour
{
    [SerializeField] private Player player;
    public static List<GameObject> explosiveBloodAmount = new List<GameObject>();
    private bool resetMove;

    void Start()
    {
        resetMove = true;
    }


  //  float randomX;
  //  float randomZ;
    void Update()
    {
        if (player)
            player.stats.health -= 1f * Time.deltaTime;

        if (!resetMove) return;
     //   randomX = Random.Range(-1f, 1f);
     //   randomZ = Random.Range(-1f, 1f);
        foreach (GameObject explosive in explosiveBloodAmount)
        {
            if(explosive)
                StartCoroutine(MoveExplosive(explosive, 2f));
        }
        resetMove = false;

    }

    private IEnumerator MoveExplosive(GameObject explosive, float timer)
    {
        Vector3 startLocation = explosive.transform.position;
        Vector3 endLocation = startLocation + new Vector3(0, .4f, 0);

        Vector3 startRotation = explosive.transform.eulerAngles; // current rotation
        Vector3 endRotation = startRotation + new Vector3(0, 90f, 0); // example: rotate 180 degrees on Y

        float time = 0;

        while (time < timer)
        {
            if (!explosive) yield break;
            explosive.transform.position = Vector3.Lerp(startLocation, endLocation, time / timer);
            explosive.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), time / timer);

            time += Time.deltaTime; 
            yield return null;
        }

        time = 0;
        startLocation = explosive.transform.position;
        endLocation = startLocation - new Vector3(0, .4f, 0);

        startRotation = explosive.transform.eulerAngles;
        endRotation = startRotation + new Vector3(0, 90f, 0);

        while (time < timer)
        {
            if (!explosive) yield break;
            explosive.transform.position = Vector3.Lerp(startLocation, endLocation, time / timer);
            explosive.transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRotation), Quaternion.Euler(endRotation), time / timer);

            time += Time.deltaTime;
            yield return null;
        }

        resetMove = true;


    }
}
