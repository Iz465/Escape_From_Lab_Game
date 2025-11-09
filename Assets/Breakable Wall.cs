using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private List<GameObject> wallChunk = new List<GameObject>();
    [SerializeField] public AudioClip hitSound;
    public AudioSource audioSource;

    public static bool canHitWall;
    private static float staticHealth;
    

    private void Start()
    {
        staticHealth = 60;
        audioSource = GetComponent<AudioSource>();
    }

    public void WallDamage(float damage)
    {
        if (hitSound) audioSource.PlayOneShot(hitSound);

        Debug.Log($"Health before: {staticHealth}");
        staticHealth -= damage;
        Debug.Log($"Health after: {staticHealth}");
        if (staticHealth <= 0)
            BreakWall();
    }

    private void BreakWall()
    {
        staticHealth = 60;
        Player player = FindAnyObjectByType<Player>();
        if (player && hitSound) player.audioSource.PlayOneShot(hitSound);


        Collider collider = GetComponent<Collider>();
        Bounds bounds = collider.bounds;

    
        foreach (GameObject chunk in wallChunk)
        {
            Vector3 randomPosition = new Vector3(Random.Range(bounds.min.x, bounds.max.x), 
                Random.Range(bounds.min.y, bounds.max.y), 
                Random.Range(bounds.min.z, bounds.max.z));
            GameObject ragdoll = Instantiate(chunk, randomPosition, Quaternion.identity);
            Rigidbody rigid = ragdoll.GetComponent<Rigidbody>();
            rigid.AddExplosionForce(15, ragdoll.transform.position, 5);
            StartCoroutine(DestroyRubble(ragdoll, 5));
       
        }

      
        Destroy(gameObject);
    }
    
    private IEnumerator DestroyRubble(GameObject rubble,float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(rubble);

    }
}

