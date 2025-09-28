using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class DangerSpawner : MonoBehaviour
{
    public GameObject danger;
    public float dropSpeed;
    public float dropDelay;
    float lastDrop = 0;
    // Update is called once per frame
    void Update()
    {
        if(lastDrop < Time.time)
        {
            lastDrop = Time.time+dropDelay;

            GameObject newDanger = Instantiate(danger);

            float randomX = Random.Range(510.1f, 1043.7f);
            float randomZ = Random.Range(-231f, 255.3f);
            newDanger.transform.position = new Vector3(randomX, 67f,randomZ);
            newDanger.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -dropSpeed, 0);

            Destroy(newDanger, 10f);
        }
    }
}
