using Unity.VisualScripting;
using UnityEngine;


public class FallingTile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print("oncollisionenter");
        if (collision.transform.CompareTag("Player"))
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print("oncontrollercolliderhit");
        if (hit.transform.CompareTag("Player"))
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("ontriggerenter");
        if (other.transform.CompareTag("Player"))
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
