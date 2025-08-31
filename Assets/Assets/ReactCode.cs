using UnityEngine;

public class ReactCode : MonoBehaviour
{
    Speed speedCode;
    void Start()
    {
        speedCode = transform.parent.GetComponent<Speed>();
    }
    void React(Collider collider)
    {
        if (speedCode.highSpeedMode) return;

        Rigidbody body;
        if (collider.transform.TryGetComponent(out body))
        {
            if (body.linearVelocity.magnitude > 20)
            {
                speedCode.HighSpeedMode();
            }
        }
    }

    private void OnTriggerEnter(Collider other) => React(other);
}
