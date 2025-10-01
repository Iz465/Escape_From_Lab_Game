using Unity.VisualScripting;
using UnityEngine;

public class PlayerHitDetection : MonoBehaviour
{
    [SerializeField]
    private float damage;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Something has been hit : {collision.gameObject}");
        Player player = collision.gameObject.GetComponent<Player>();
        if (player)
            player.TakeDamage(damage);
        Destroy(gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        /*     Debug.Log("Trigger activated");
             Vector3 playerDistance = (other.transform.position - transform.parent.position);
             float distance = playerDistance.magnitude;
             var scale = transform.localScale;
             scale.z = distance;
             transform.localScale = scale; */



    }

 //   private void OnTriggerStay(Collider other)
 //   {
 //       Debug.Log("Beam Hitting");
 //   }
    float beamLength;

    private void Start()
    {
        beamLength = 20f;
        var scale = transform.parent.localScale;
        scale.z = beamLength;
        transform.parent.localScale = scale;
    }

    [SerializeField] private LayerMask playerLayer;
    private void Update()
    {

        RaycastHit hit;




        if (Physics.SphereCast(transform.parent.position, .5f, transform.parent.forward, out hit, beamLength, playerLayer))
            beamLength = hit.distance;

        else
            beamLength = Mathf.Lerp(beamLength, 20f, Time.deltaTime / 1f);
           
        
    


        var scale = transform.parent.localScale;
        scale.z = beamLength;
        transform.parent.localScale = scale;


        // Rotate the beam to face the player
        if (hit.collider != null)
        {
            // Rotate directly towards the hit point (player)
            transform.parent.LookAt(hit.point);
        }
        else
        {
            // If nothing hit, just align with enemy forward
      //      transform.parent.rotation = transform.parent.parent.rotation;
        }


    }

}

