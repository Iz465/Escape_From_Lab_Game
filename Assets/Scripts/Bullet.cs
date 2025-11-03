using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;
    float lifeSpan = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Bullet")) return;

        if (collision.transform.CompareTag("Player"))
        {
            PlayerInfo playerInfo = collision.transform.GetComponent<PlayerInfo>();
            playerInfo.health -= 15;
        }
        Destroy(gameObject);

    }

    //private void Update()
    //{
        /*Vector3 targetPlace = rb.linearVelocity * Time.timeScale * Time.deltaTime;

        RaycastHit hit;


        Debug.DrawRay(transform.position, targetPlace);
        if(Physics.Raycast(transform.position, targetPlace, out hit))
        {
            print(hit.transform);
            if (hit.collider == null) return;

            if (hit.collider.CompareTag("Player"))
            {
                PlayerInfo playerInfo = hit.transform.GetComponent<PlayerInfo>();
                playerInfo.health -= 15;
            }
            Destroy(gameObject);
        }

        lifeSpan += Time.deltaTime;
        print(lifeSpan);

        if(lifeSpan > 5)
        {
            print("bullet old");
            Destroy(gameObject);
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        print(collision.transform.name);
        print(collision.transform.tag);
        if (collision.transform.CompareTag("Player"))
        {
            PlayerInfo playerInfo = collision.transform.GetComponent<PlayerInfo>();
            playerInfo.health -= 15;
        }

        Destroy(gameObject);
    }*/
}
