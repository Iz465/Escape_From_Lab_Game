using UnityEngine;

public class Lava : MonoBehaviour
{
    void DamagePlayer(Transform obj)
    {
        PlayerInfo info;
        obj.TryGetComponent<PlayerInfo>(out info);

        if (info == null) ;
        info = obj.parent.GetComponent<PlayerInfo>();

        info.health -= 1;
    }


    private void OnCollisionStay(Collision collision)
    {
        print("collided");
        Transform obj = collision.transform;

        print(obj.tag);
        if (!obj.CompareTag("Player")) return;

        DamagePlayer(obj);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collided");
        Transform obj = collision.transform;

        print(obj.tag);
        if (!obj.CompareTag("Player")) return;

        DamagePlayer(obj);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("triggerEnter");
        Transform obj = other.transform;

        print(obj.tag);
        if (!obj.CompareTag("Player")) return;

        DamagePlayer(obj);
    }

    private void OnTriggerStay(Collider other)
    {
        print("triggerStay");
        Transform obj = other.transform;

        print(obj.tag);
        print(obj.name);
        if (!obj.CompareTag("Player")) return;

        DamagePlayer(obj);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print("collided");
        Transform obj = hit.transform;

        print(obj.tag);
        if (!obj.CompareTag("Player")) return;

        DamagePlayer(obj);
    }
}
