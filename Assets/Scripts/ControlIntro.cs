using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlIntro : MonoBehaviour
{
    float w, a, s, d, space, mouse;
    [SerializeField] Text guide;
    [SerializeField] GameObject characterSceen;

    void Start()
    {
        guide = gameObject.GetComponent<Text>();
        
        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad()
    {
        MainMenu.WaitForLoad();
        
        if (MainMenu.saveFile.playedBefore)
            gameObject.SetActive(false);

        if (MainMenu.saveFile.initialTutorial)
            gameObject.SetActive(false);
        yield return null;
    }

    void ShowControls()
    {
        if (w < 5)
        {
            guide.text = "Hold W to move forward";
        }
        else if (a < 5)
        {
            guide.text = "Hold A to move left";
        }
        else if (s < 5)
        {
            guide.text = "Hold S to move backwards";
        }
        else if (d < 5)
        {
            guide.text = "Hold D to move right";
        }
        else if (space < 5)
        {
            guide.text = "Press SPACE to jump";
        }
        else if (mouse < 5)
        {
            guide.text = "Move your mouse to look around";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void ReadControls()
    {
        if (Input.GetKey(KeyCode.W))
            w += Time.deltaTime;

        if(w >= 5 && Input.GetKey(KeyCode.A))
            a += Time.deltaTime;

        if(a >= 5 && Input.GetKey(KeyCode.S))
            s += Time.deltaTime;

        if(s >= 5 && Input.GetKey(KeyCode.D))
            d += Time.deltaTime;

        if(d >= 5 && Input.GetKey(KeyCode.Space))
            space += Time.deltaTime;

        if (space >= 5 && Input.mousePositionDelta != Vector3.zero)
            mouse += Time.deltaTime;

        if(mouse >= 5)
        {
            MainMenu.saveFile.initialTutorial = true;
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (characterSceen.activeSelf) return;

        ShowControls();
        ReadControls();
    }
}
