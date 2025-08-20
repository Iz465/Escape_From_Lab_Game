using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public CharacterController controller;
    public float walkSpeed;
    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;

        controller.Move(direction.normalized * walkSpeed * Time.deltaTime);
    }
}
