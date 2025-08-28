using UnityEngine;

public class SpawnPower : BasePower
{
    [SerializeField]
    private float yLoc;
    [SerializeField]
    private float zLoc;
    private Vector3 forwardPos;




    protected override void FirePower(GameObject power)
    {
        var powerLoc = power.transform.position;

        power.transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 0));
   
        if (zLoc != 0)
        {
            forwardPos = cam.transform.forward * zLoc;
            powerLoc.z = forwardPos.z;
            power.transform.position += forwardPos;
        }

        if (yLoc != 0)
        {
            var upPos = power.transform.position;
            upPos.y = yLoc;
            power.transform.position = upPos;
        }



    }
}

