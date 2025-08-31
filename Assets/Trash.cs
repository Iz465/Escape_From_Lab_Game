using System.Collections;
using UnityEngine;

public class Trash : MonoBehaviour
{
    Vector3 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos = transform.position;
    }


    private void OnCollisionEnter(Collision collision)
    {
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, 0, -25);
    }
}
