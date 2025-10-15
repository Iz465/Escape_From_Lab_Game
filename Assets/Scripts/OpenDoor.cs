using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Transform interact;
    Transform plr;

    Transform doorAngle;
    Transform door;
    Transform handle;
    public float angle;
    float handleDistance;

    bool isOpen, startMove;
    float process;
    void Start()
    {
        //handle = transform.Find("Handle");
        //door = transform.Find("Door");
        doorAngle = transform.Find("doorAngle");
        //angle = doorAngle.localRotation.y;
        //angle = doorAngle.rotation.y;
        print(transform.name+" "+angle.ToString());
        interact = transform.Find("Canvas");

        //handleDistance = (handle.position - doorAngle.position).magnitude;
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

    bool IsLookingAtDoor()
    {
        Vector3 doorDirection = (transform.position - plr.position).normalized;
        Vector3 playerDirection = plr.forward;

        bool isLooking = Vector3.Dot(doorDirection, playerDirection) > 0.5f;
        return isLooking;
    }

    void DisplayInteraction()
    {
        if((plr.position - transform.position).magnitude < 10)
            interact.gameObject.SetActive(true);
        else
            interact.gameObject.SetActive(false);
    }


    bool IsAtDoor()
    {
        bool isLooking = IsLookingAtDoor();
        bool isAtDoor = ((plr.position - transform.position).magnitude < 10);

        return isAtDoor;
    }

    void OpenClose()
    {
        if (!startMove) return;

        if (isOpen)
        {
            angle -= 90 * Time.deltaTime;

            if(process >= 1)
            {
                isOpen = false;
                process = 0;
                startMove = false;
            }
        }else{
            angle += 90 * Time.deltaTime;

            if (process >= 1)
            {
                isOpen = true;
                process = 0;
                startMove = false;
            }
        }

        print(angle);
        process += Time.deltaTime;
        doorAngle.rotation = Quaternion.Euler(0, angle, 0);
        print("angle: "+doorAngle.rotation.y.ToString());
        //door.rotation = doorAngle.rotation;
        //door.position = doorAngle.position + doorAngle.forward * door.lossyScale.z;
        //handle.position = doorAngle.position + doorAngle.forward * handleDistance;
        //handle.LookAt(doorAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null) return;
        DisplayInteraction();

        if(Input.GetKeyDown(KeyCode.E) && IsAtDoor())
        {
            print("Starting angle: "+angle.ToString());
            startMove = true;
        }

        OpenClose();
    }
}
