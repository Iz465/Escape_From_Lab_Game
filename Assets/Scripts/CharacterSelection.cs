using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Integrations.Match3;
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

    string chosenCharacter = null;
    Transform cam;
    [SerializeField] List<Transform> camPositions = new List<Transform>();
    public float cameraMoveDuration;
    [SerializeField] GameObject controlInfo;

    private void Start()
    {
        transform.Find("Speed").GetComponent<Button>().onClick.AddListener(Speed);
        transform.Find("Ice").GetComponent<Button>().onClick.AddListener(Ice);
        transform.Find("Blood").GetComponent<Button>().onClick.AddListener(Blood);
        transform.Find("Warrior").GetComponent<Button>().onClick.AddListener(Warrior);
        transform.Find("Confirm").GetComponent<Button>().onClick.AddListener(Confirm);
        print("controls");
        cam = GameObject.Find("Camera").transform;
        StartCoroutine(WaitForLoad());
        print("loaded");
    }

    IEnumerator WaitForLoad()
    {
        while (MainMenu.saveFile == null)
            yield return new WaitForSeconds(0.1f);

        saveFile = MainMenu.saveFile;
        print(JsonUtility.ToJson(saveFile));
    }

    IEnumerator SelectCharacter()
    {
        Text text = transform.Find("Confirm").GetComponentInChildren<Text>();
        text.text = " Please select a character";

        yield return new WaitForSeconds(2);

        text.text = "Confirm";
    }

    public void MakeCharacter(string character)
    {
        print(character);
        if (character == "Speed")
            Speed();
        if(character == "Ice")
            Ice();

        if( character == "Blood")
            Blood();
        
        if(character == "Warrior")
            Warrior();

        Confirm();
    }

    float progress = 0;
    bool move = false;
    Vector3 startPosition;
    Quaternion startRotation;
    void MoveCamera()
    {
        int index = 0;

        if (chosenCharacter == "Speed")
            index = 0;

        if (chosenCharacter == "Teleportation")
            index = 1;

        if (chosenCharacter == "Ice")
            index = 2;

        if(chosenCharacter == "Blood")
            index = 3;

        if(chosenCharacter == "Warrior")
            index = 4;

        if(chosenCharacter == "Fire")
            index = 5;


        Quaternion rotation = camPositions[index].rotation;
        Quaternion finalRotation = Quaternion.Lerp(startRotation, rotation, progress);
        Vector3 pos = Vector3.Lerp(startPosition, camPositions[index].position, progress);

        progress += Time.deltaTime/cameraMoveDuration;
        cam.transform.SetPositionAndRotation(pos, finalRotation);

        if(progress >= 1)
        {
            progress = 0;
            move = false;
        }
    }

    public void Confirm()
    {
        print("confirming");
        if(chosenCharacter == null)
        {
            StartCoroutine(SelectCharacter());
            return;
        }

        print("new character");
        Transform newPlayerModel = Instantiate(playerCharModel);
        saveFile.characterChosen = chosenCharacter;
        
        if(chosenCharacter == "Speed")
        {
            newPlayerModel.GetComponent<Move>().useOtherScript = true;
            Destroy(newPlayerModel.GetComponent<Ice>());
        }
        if(chosenCharacter == "Ice")
        {
            newPlayerModel.GetComponent <Move>().useOtherScript = true;
            Destroy(newPlayerModel.GetComponent<Speed>());
        }

        if(chosenCharacter == "Blood")
        {
            newPlayerModel = Instantiate(bloodCharacterModel);
            newPlayerModel.GetComponent<Move>().useOtherScript = false;
        }

        if(chosenCharacter == "Warrior")
        {
            newPlayerModel = Instantiate(warriorCharacterModel);
            newPlayerModel.GetComponent<Move>().useOtherScript = false;
        }

        print("made character");
        FinishSetup(newPlayerModel);
    }
    void Speed() 
    { 
        chosenCharacter = "Speed"; 
        move = true;
        startRotation = cam.rotation;
        startPosition = cam.position;
    }

    void Ice()
    {
        chosenCharacter = "Ice";
        move = true;
        startRotation = cam.rotation;
        startPosition = cam.position;
    }

    void Blood()
    {
        chosenCharacter = "Blood";
        move = true;
        startRotation = cam.rotation;
        startPosition = cam.position;
    }

    void Warrior()
    {
        chosenCharacter = "Warrior";
        move = true;
        startRotation = cam.rotation;
        startPosition = cam.position;
    }


    void FinishSetup(Transform newPlayerModel)
    {
        newPlayerModel.parent = null;
        PlayerInfo info = newPlayerModel.AddComponent<PlayerInfo>();

        info.health = 100;
        info.stamina = 100;
        info.maxHealth = 100;

        newPlayerModel.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        Debug.Log("SPAWNING");
        print(saveFile.characterChosen);
        Destroy(GameObject.Find("Camera"));
        gameObject.SetActive(false);
        controlInfo.SetActive(true);

    }

    private void Update()
    {
        if (!move)
        {
            progress = 0;
            return;
        }

        MoveCamera();
    }
}
