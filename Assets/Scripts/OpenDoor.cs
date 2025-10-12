using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Transform interact;
    Transform plr;
    void Start()
    {
        interact = transform.Find("Canvas");
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
    }

    void DisplayInteraction()
    {
        if((plr.position - transform.position).magnitude < 10)
            interact.gameObject.SetActive(true);
        else
            interact.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null) return;
        DisplayInteraction();
    }
}
