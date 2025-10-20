using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    private Player player;
    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }
    private void Update()
    {
        if (!player) return;

        Vector3 playerDirection = player.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerDirection), Time.deltaTime * 5);

    }
}
