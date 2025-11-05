using UnityEngine;

public class ObstaclePusher : MonoBehaviour
{
    [SerializeField]
    private float HitMag = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody Rigid_Body = hit.collider.attachedRigidbody;
        if(Rigid_Body != null)
        {
            Vector3 ForceDirect = hit.gameObject.transform.position - transform.position;
            ForceDirect.y = 0;
            ForceDirect.Normalize();

            Rigid_Body.AddForceAtPosition(ForceDirect * HitMag, transform.position, ForceMode.Impulse);
        }
    }


}
