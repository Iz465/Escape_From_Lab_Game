using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(WaitForPlayer());
    }

    IEnumerator WaitForPlayer()
    {
        while (!GameObject.FindGameObjectWithTag("Player"))
        {
            yield return null;
        }
        Destroy(GameObject.Find("Camera"));

        GameObject plr = GameObject.FindGameObjectWithTag("Player");

        plr.transform.position = transform.position;
    }
}
