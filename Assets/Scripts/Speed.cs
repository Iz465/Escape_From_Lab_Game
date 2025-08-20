using UnityEngine;

public class Speed : MonoBehaviour
{
    Move movement;
    [SerializeField]
    float normalWalk,normalRun, highSpeedWalk, highSpeedRun, dashSpeed, dashDuration;
    float dashStart;

    bool highSpeedMode;
    [SerializeField] float highSpeedModeScale;

    void Start()
    {
        movement = transform.GetComponent<Move>();
    }

    void Run()
    {
        movement.velocity = movement.direction * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!highSpeedMode)
            {
                movement.velocity *= normalRun;
            }
            else
            {
                movement.velocity *= highSpeedRun;
            }
        }
        else
        {
            if (!highSpeedMode)
            {
                movement.velocity *= normalWalk;
            }
            else
            {
                movement.velocity *= highSpeedWalk;
            }
        }

        //right click to (de)activate
        if (Input.GetMouseButtonDown(1))
        {
            highSpeedMode = !highSpeedMode;
            Time.timeScale = highSpeedMode ? highSpeedModeScale : 1;
        }
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dashStart = Time.time;
        }

        if(dashStart+dashDuration > Time.time)
        {
            movement.velocity = movement.direction * Time.deltaTime * dashSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Dash();
    }
}
