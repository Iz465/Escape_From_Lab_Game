using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class DangerSpawner : MonoBehaviour
{
    public GameObject danger;
    public float dropSpeed;
    public float dropDelay;
    float lastDrop = 0;

    float topZ, bottomZ, topX, bottomX;

    private void Start()
    {
        Vector3 pos = transform.position;
        Vector3 size = transform.lossyScale;
        topZ = pos.z + size.z/2;
        bottomZ = pos.z - size.z/2;
        topX = pos.x + size.x/2;
        bottomX = pos.x - size.x / 2;
    }
    // Update is called once per frame
    void Update()
    {
        if(lastDrop < Time.time)
        {
            lastDrop = Time.time+dropDelay;

            GameObject newDanger = Instantiate(danger);

            float randomX = Random.Range(bottomX, topX);
            float randomZ = Random.Range(bottomZ, topZ);
            newDanger.transform.position = new Vector3(randomX, 67f,randomZ);
            newDanger.GetComponent<Rigidbody>().linearVelocity = new Vector3(0, -dropSpeed, 0);

            Destroy(newDanger, 10f);
        }
    }
}
