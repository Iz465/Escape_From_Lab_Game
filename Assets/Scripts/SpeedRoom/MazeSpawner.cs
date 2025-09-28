using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    Transform plr;
    PlayerInfo playerInfo;
    List<Vector3> spawnPositions = new List<Vector3> ();

    private void Start()
    {
        foreach (Transform pos in transform)
        {
            spawnPositions.Add(pos.position);
            print(pos.position);
        }
        StartCoroutine(WaitForPlayer());

    }

    void SpawnPlayer()
    {
        Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count - 1)];
        plr.GetComponent<CharacterController>().enabled = false;
        plr.position = spawnPos;
        plr.GetComponent<CharacterController>().enabled = true;
        print("spawned player at "+ spawnPos.ToString());
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
        playerInfo = plr.GetComponent<PlayerInfo>();

        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if (!plr) return;
        if(playerInfo.health <= 0)
            SpawnPlayer();

        if (plr.position.y < -200)
        {
            SpawnPlayer();
        }
    }
}
