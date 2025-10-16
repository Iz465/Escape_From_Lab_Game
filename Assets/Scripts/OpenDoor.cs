using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Transform interact;
    Transform plr;

    Transform doorAngle;
    Transform door;
    Transform handle;
    float angle;
    float handleDistance;

    bool isOpen, startMove;
    float process;
    void Start()
    {
        //get local euler angles as shown in inspector (360 degrees)
        doorAngle = transform.Find("doorAngle");
        angle = doorAngle.localEulerAngles.y;
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

        process += Time.deltaTime;
        //set rotation as shown in inspector (360 degrees)
        doorAngle.localRotation = Quaternion.Euler(0, angle, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (plr == null) return;

        if(interact != null)
            DisplayInteraction();

        if(Input.GetKeyDown(KeyCode.E) && IsAtDoor())
        {
            print("Starting angle: "+angle.ToString());
            startMove = true;
        }

        OpenClose();
    }
}
