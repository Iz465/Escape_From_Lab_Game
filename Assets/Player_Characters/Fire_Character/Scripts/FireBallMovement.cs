using UnityEngine;

public class FireBallMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 Movement = this.transform.position + Camera.main.transform.position;
        Movement.y = 0;
        this.GetComponent<Rigidbody>().AddRelativeForce(Movement * 10f);
    }
}
