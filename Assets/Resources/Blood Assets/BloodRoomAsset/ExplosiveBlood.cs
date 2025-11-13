using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBlood : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private AudioClip explosionSound;

    private Player playerAudio;
    private void Start()
    {
        BloodLevel.explosiveBloodAmount.Add(gameObject);
        playerAudio = FindAnyObjectByType<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {

        if (playerAudio)
            playerAudio.audioSource.PlayOneShot(explosionSound);


        Collider[] bodies = Physics.OverlapSphere(transform.position, 5f);
        HashSet<GameObject> unique = new HashSet<GameObject>();
        foreach (Collider body in bodies) 
        {
            GameObject uniqueBody = body.transform.root.gameObject;
            Player player = uniqueBody.GetComponent<Player>();
            navmeshtestscript enemy = uniqueBody.GetComponent<navmeshtestscript>();
            if (player)
                unique.Add(uniqueBody);
        }

        foreach (GameObject body in unique)
        {
            Debug.Log($"Body has been hit! : {body}");
            Player player = body.GetComponent<Player>();
            navmeshtestscript enemy = body.GetComponent<navmeshtestscript>();
            if (player)
                player.TakeDamage(30);
            if (enemy)
                enemy.TakeDamage(30);
        }
          


        BloodLevel.explosiveBloodAmount.Remove(gameObject);

        
        Instantiate(explosion, transform.position, Quaternion.identity);
  
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
