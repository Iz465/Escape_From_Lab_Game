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

    string chosenCharacter;

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
    }

    public void Confirm()
    {
        StartCoroutine(WaitForLoad());

        if(chosenCharacter == null)
        {
            StartCoroutine(SelectCharacter());
            return;
        }

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
            newPlayerModel.GetComponent<Move>().useOtherScript = true;
        }

        FinishSetup(newPlayerModel);
    }
    void Speed() => chosenCharacter = "Speed";

    void Ice() => chosenCharacter = "Ice";

    void Blood() => chosenCharacter = "Blood";

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
