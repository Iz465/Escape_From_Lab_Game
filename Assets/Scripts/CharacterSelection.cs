using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Transform playerCharModel;

    private void Start()
    {
        transform.Find("Speed").GetComponent<Button>().onClick.AddListener(Speed);
        transform.Find("Ice").GetComponent<Button>().onClick.AddListener(Ice);
    }


    void Speed()
    {
        Transform newPlayerModel = Instantiate(playerCharModel);
        Speed speed = newPlayerModel.AddComponent<Speed>();

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

        newPlayerModel.GetComponent<Move>().useOtherScript = true;
        FinishSetup(newPlayerModel);
    }

    void Ice()
    {
        Transform newPlayerModel = Instantiate(playerCharModel);
        Ice ice = newPlayerModel.AddComponent<Ice>();

        ice.iceSpeed = 15;
        ice.walkSpeed = 3;
        ice.characterHeight = 5;

        ice.iceWall = Resources.Load<Transform>("Ice wall");
        ice.iceFloor = Resources.Load<Transform>("iceFloor");
        ice.iceSpike = Resources.Load<Transform>("spike");

        newPlayerModel.GetComponent<Move>().useOtherScript = true;
        FinishSetup(newPlayerModel);
    }

    void FinishSetup(Transform newPlayerModel)
    {
        newPlayerModel.parent = null;
        PlayerInfo info = newPlayerModel.AddComponent<PlayerInfo>();

        info.health = 100;
        info.stamina = 100;
        info.maxHealth = 100;

        Destroy(GameObject.Find("Camera"));
        gameObject.SetActive(false);
    }
}
