using Unity.VisualScripting;
using UnityEngine;


public class FallingTile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.CompareTag("Player"))
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
