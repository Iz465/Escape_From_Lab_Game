using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Speed : PlayerInfo
{
    Move movement;
    PlayerInfo playerInfo;
    Transform cam;

    public float normalWalk,normalRun, highSpeedWalk, highSpeedRun, dashSpeed, dashDuration;
    public float normalRunCost, highSpeedRunCost, dashCost, phazeCost;
    public float regenRate;
    public float highSpeedModeScale, highSpeedModeCost;
    public Text staminaText, healthText;

    float dashStart;
    public bool highSpeedMode, phazeMode;
    float lastPowerUsage;

    bool phazePoint = false; //true and false will indicate left and right camera position

    void Start()
    {
        movement = transform.GetComponent<Move>();
        playerInfo = transform.GetComponent<PlayerInfo>();
        cam = transform.Find("Main Camera").transform;

        Transform stats = GameObject.Find("Canvas").transform.Find("Stats");

        staminaText = stats.Find("Stamina").GetComponent<Text>();
        healthText = stats.Find("Health").GetComponent <Text>();

        meleeAttackCooldown = 0.1f;
        meleeAttackAnimation = "Melee";
        animator = transform.GetComponent<Animator>();
        attackDuration = 0.33f;
    }

    void Run()
    {
        if(stamina <= 0)
            highSpeedMode = false;

        movement.velocity = movement.direction * Time.deltaTime;
        Vector3 vel = movement.velocity;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!highSpeedMode)
            {
                playerInfo.stamina -= normalRunCost * Time.deltaTime;
                vel.x *= normalRun;
                vel.z *= normalRun;
            }
            else
            {
                playerInfo.stamina -= (highSpeedRunCost + highSpeedModeCost) * Time.deltaTime;
                //movement.velocity *= highSpeedRun;
                vel.x *= highSpeedRun;
                vel.z *= highSpeedRun;
            }
            lastPowerUsage = Time.time;
        }
        else
        {
            if (!highSpeedMode)
            {
                //movement.velocity *= normalWalk;
                vel.x *= normalWalk;
                vel.z *= normalWalk;
            }
            else
            {
                playerInfo.stamina -= highSpeedModeCost * Time.deltaTime;
                lastPowerUsage = Time.time;
                //movement.velocity *= highSpeedWalk;
                vel.x *= highSpeedWalk;
                vel.z *= highSpeedWalk;
            }
        }

        movement.velocity = vel;

        //staminaText.text = playerInfo.stamina.ToString()+" stamina";

        //right click to (de)activate
        if (Input.GetMouseButtonDown(1))
            HighSpeedMode();

        //left click to attack
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(MeleeAttack(animator));
    }

    public void HighSpeedMode()
    {
        if (stamina < 0) return;
        highSpeedMode = !highSpeedMode;
        Time.timeScale = highSpeedMode ? highSpeedModeScale : 1;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerInfo.stamina -= dashCost;
            lastPowerUsage = Time.time;
            staminaText.text = playerInfo.stamina.ToString() + " stamina";
            dashStart = Time.time;
        }

        if(dashStart+dashDuration > Time.time)
        {
            movement.velocity = movement.direction * Time.deltaTime * dashSpeed;
        }
    }

    void Phaze()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            phazeMode = !phazeMode;

            GameObject[] objects = GameObject.FindGameObjectsWithTag("PhazeObject");
            foreach(GameObject obj in objects)
            {
                if (phazeMode)
                {
                    obj.layer = LayerMask.NameToLayer("Phaze");
                    MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                    Material mat = renderer.material;
                    Color matColor = mat.color;
                    matColor.a = 0.5f;
                    mat.color = matColor;
                    renderer.material = mat;
                }
                else{
                    obj.layer = LayerMask.NameToLayer("Default");
                    MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
                    Material mat = renderer.material;
                    Color matColor = mat.color;
                    matColor.a = 1f;
                    mat.color = matColor;
                    renderer.material = mat;
                }
            }
        }

        if (phazeMode)
        {
            cam.position += phazePoint ? cam.right*0.1f : cam.right *-0.1f;
            phazePoint = !phazePoint;
            playerInfo.stamina -= phazeCost;
            lastPowerUsage = Time.time;
        }
    }

    void Heal()
    {
        if (playerInfo.health < playerInfo.maxHealth)
            if (playerInfo.lastDamageTime < Time.time - 5)
                playerInfo.health += regenRate * Time.deltaTime;


        healthText.text = playerInfo.health.ToString() + " health";
    }

    void Regen()
    {
        if(lastPowerUsage < Time.time-5 && playerInfo.stamina < 100)
            playerInfo.stamina += regenRate * Time.deltaTime;
    }


    // Update is called once per frame
    void Update()
    {
        Run();
        Dash();
        Phaze();
        Heal();
        Regen();

        if (animator.GetBool(meleeAttackAnimation))
        {

            DamageEnemy();
        }
    }
}
