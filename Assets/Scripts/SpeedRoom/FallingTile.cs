using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class FallingTile : MonoBehaviour
{
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
