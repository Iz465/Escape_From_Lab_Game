using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    bool spawned = false;
    Transform plr;
    public string room;
    public Text guide;
    public AudioSource spokenGuide;

    private void Start()
    {
        StartCoroutine(WaitForPlayer());
        Introduce();
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
        plr.GetComponent<Move>().enabled = false;

        if (plr.GetComponent<Speed>())
        {
            plr.GetComponent<Speed>().spawnPosition = transform;
            
        }

        plr.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        plr.position = transform.position;
        plr.GetComponent<Move>().enabled = true;
        spawned = true;
        plr.GetComponent<CharacterController>().enabled = true;
        print("player spawned");
    }

    void Introduce()
    {
        if(room == "Speed1")
        {
            if (MainMenu.saveFile.havePlayedSpeedRoom1) return;
            spokenGuide.Play();
            guide.text = "Press E to dash";
            MainMenu.saveFile.dash = true;
        }
    }

    private void Update()
    {
        if (!spawned) return;

        if(plr.position.y < -200)
        {
            print("player too low");
            plr.GetComponent<Move>().enabled = false;
            plr.position = transform.position;
            plr.GetComponent<Move>().enabled = true;
        }
    }
}
