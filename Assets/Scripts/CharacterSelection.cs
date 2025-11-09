using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Transform playerCharModel;
    [SerializeField] Transform bloodCharacterModel;
    [SerializeField] Transform warriorCharacterModel;
    SaveablePlayer saveFile;

    private void Start()
    {
        transform.Find("Speed").GetComponent<Button>().onClick.AddListener(Speed);
        transform.Find("Ice").GetComponent<Button>().onClick.AddListener(Ice);
        transform.Find("Blood").GetComponent<Button>().onClick.AddListener(Blood);
        transform.Find("Warrior").GetComponent<Button>().onClick.AddListener(Warrior);
        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad()
    {
        while (MainMenu.saveFile == null)
            yield return null;

        saveFile = MainMenu.saveFile;
        print(JsonUtility.ToJson(saveFile));
    }

    public void MakeCharacter(string character)
    {
        print(character);
        if (character == "Speed")
            Speed();
        if(character == "Ice")
            Ice();
    }

    void Speed()
    {
        StartCoroutine(WaitForLoad());
        Transform newPlayerModel = Instantiate(playerCharModel);
        /*Speed speed = newPlayerModel.AddComponent<Speed>();

        speed.normalWalk = 7;
        speed.normalRun = 150;
        speed.highSpeedWalk = 500;
        speed.highSpeedRun = 3000;
        speed.dashSpeed = 30;
        speed.dashDuration = 0.02f;

        speed.normalRunCost = 2.5f;
        speed.highSpeedRunCost = 5;
        speed.dashCost = 10;
        speed.phazeCost = 7.5f;

        speed.regenRate = 15;

        speed.highSpeedModeScale = 0.01f;
        speed.highSpeedModeCost = 2;
        */
        saveFile.characterChosen = "Speed";
        print(JsonUtility.ToJson(saveFile));
        newPlayerModel.GetComponent<Move>().useOtherScript = true;
        Destroy(newPlayerModel.GetComponent<Ice>());
        FinishSetup(newPlayerModel);
    }

    void Ice()
    {
        Transform newPlayerModel = Instantiate(playerCharModel);
        newPlayerModel.GetComponent <Move>().useOtherScript = true;
        /*Ice ice = newPlayerModel.AddComponent<Ice>();

        ice.iceSpeed = 15;
        ice.walkSpeed = 10;
        ice.characterHeight = 5;

        ice.iceWall = Resources.Load<Transform>("Ice wall");
        ice.iceFloor = Resources.Load<Transform>("iceFloor");
        ice.iceSpike = Resources.Load<Transform>("spike");
        */
        MainMenu.saveFile.characterChosen = "Ice";
        Destroy(newPlayerModel.GetComponent<Speed>());
        FinishSetup(newPlayerModel);
    }

    void Blood()
    {
        Transform newPlayerModel = Instantiate(bloodCharacterModel);
        FinishSetup(newPlayerModel);

    }

    void Warrior()
    {
        Transform newPlayerModel = Instantiate(warriorCharacterModel);
        FinishSetup(newPlayerModel);
     

    }

  
    void FinishSetup(Transform newPlayerModel)
    {
        newPlayerModel.parent = null;
        PlayerInfo info = newPlayerModel.AddComponent<PlayerInfo>();

        info.health = 100;
        info.stamina = 100;
        info.maxHealth = 100;

        newPlayerModel.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;

        print(saveFile.characterChosen);
        Destroy(GameObject.Find("Camera"));
        gameObject.SetActive(false);

    }


}
