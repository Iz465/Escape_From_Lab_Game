using Unity.VisualScripting;
using UnityEngine;


public class FallingTile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("tile collided");
        if (collision.transform.CompareTag("Player"))
        {
            print("player detected");
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print("tile collided");
        if (hit.transform.CompareTag("Player"))
        {
            print("player detected");
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("tile collided");
        if (other.transform.CompareTag("Player"))
        {
            print("player detected");
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
