using System.Collections;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    bool spawned = false;
    Transform plr;
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
        if (GameObject.Find("Camera"))
        {
            Destroy(GameObject.Find("Camera"));
        }

        plr = GameObject.FindGameObjectWithTag("Player").transform;

        plr.position = transform.position;
        spawned = true;
    }

    private void Update()
    {
        if (!spawned) return;

        if(plr.position.y < -200)
        {
            plr.position = transform.position;
        }
    }
}
