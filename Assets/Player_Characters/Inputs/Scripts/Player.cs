using Unity.MLAgents.Integrations.Match3;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageTaken
{
    [System.Serializable]
    public struct PlayerStats
    {
        public string name;
        public float health;
        public float maxHealth;
        public float stamina;
        public float maxStamina;
    }

    [Header("Basic Player Info")]
    public PlayerStats stats;
    [HideInInspector] public static bool canDamage = true;
    [SerializeField] public ParticleSystem playerHitParticle;
    [HideInInspector] public static float abilityCooldown = 30;
    [HideInInspector] public static float ability2Cooldown = 15;

    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Player stamina regenerates over every frame.
    // Horizontal velocity of player is checked to see whether player animation state should run or be idle. 
    virtual protected void Update()
    {
        stats.stamina += 5f * Time.deltaTime;
        stats.stamina = Mathf.Clamp(stats.stamina, 0, stats.maxStamina);

        if (stats.health <= 0)
            PlayerDeath();


        Animator animator = GetComponentInChildren<Animator>();
        Move move = GetComponent<Move>();

        Vector3 movement = move.controller.velocity;  // Ignores jumping/falling
        Vector3 horizontalVelocity = new Vector3(movement.x, 0, movement.z);

        if (horizontalVelocity.magnitude > 0.1f)
            animator.SetBool("Moving", true);

        else
            animator.SetBool("Moving", false);


        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pausing for debug!");
            Debug.Break();
        }
    }

    // called whenever player takes damage
    public void TakeDamage(float damageTaken) 
    {
        if (!canDamage) return;
        stats.health -= damageTaken;
        DynamicDifficultyManager.damage += damageTaken;
        if (stats.health <= 0)
            PlayerDeath();
    }


    // when the player dies the active scene / level restarts for the player to try again
    private void PlayerDeath() 
    {
        DynamicDifficultyManager.deaths += 1;
        Debug.Log("You have died");
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
      
        SceneManager.LoadScene(scene.name);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
     //   Debug.Log($"Controller hit something : {hit.gameObject}");
    }



}
