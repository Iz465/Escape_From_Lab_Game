using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SoldierActivation : MonoBehaviour
{
    [SerializeField] List<Transform> soldiersToActivate = new List<Transform> ();
    [SerializeField] List<Transform> soldiersToDeactivate = new List<Transform> ();

    bool activated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.CompareTag("Player")) return;
        if (activated) return;
        activated = true;

        foreach (Transform t in soldiersToActivate)
        {
            if (t == null) continue;
            t.gameObject.SetActive(true);
            //t.GetComponent<PlayerTargeting>().enabled = true;
        }

        foreach(Transform t in soldiersToDeactivate)
        {
            if (t == null) continue;
            t.gameObject.SetActive(true);
            //t.GetComponent <PlayerTargeting>().enabled = false;
        }
        
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
    }
    /*void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
