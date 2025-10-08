using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Vector3 targetPlace = transform.position + (rb.linearVelocity*Time.timeScale);

        RaycastHit hit;

        if(Physics.Raycast(transform.position, targetPlace, out hit))
        {
            if (hit.collider == null) return;

            if (hit.collider.CompareTag("Player"))
            {
                PlayerInfo playerInfo = hit.transform.GetComponent<PlayerInfo>();
                playerInfo.health -= 15;
            }
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
