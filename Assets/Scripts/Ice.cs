using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ice : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public Transform iceWall;
    public Transform iceSpike;
    public Transform iceFloor;

    List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            IceWall();

        if (Input.GetKeyDown(KeyCode.E))
            SpawnSpikes();

        if(items.Count > 0)
            IceSpikes();

        if (Input.GetKeyDown(KeyCode.F))
            IceFloor();

        CleanUp();
    }

    void IceWall()
    {
        Transform newWall = Instantiate(iceWall);
        newWall.position = transform.position + transform.forward * 5;
        newWall.rotation = transform.rotation;

        Dictionary<string, object> wallData = new Dictionary<string, object>();
        wallData.Add("item", newWall);
        wallData.Add("time", 5f);

        items.Add(wallData);
    }

    void SpawnSpikes()
    {
        for (int i = 5; i <= 15; i += 5)
        {
            Transform newSpike = Instantiate(iceSpike);
            newSpike.position = transform.position - new Vector3(0,newSpike.lossyScale.y,0)+ transform.forward * i;
            transform.parent = null;

            Dictionary<string, object> spikeData = new Dictionary<string, object>();
            spikeData.Add("item", newSpike);
            spikeData.Add("time", 5f);
            spikeData.Add("height", transform.position.y);
            items.Add(spikeData);

        }
    }

    void IceSpikes()
    {
        for(int i = items.Count-1; i >= 0; i--)
        {
            Dictionary<string, object> spikeData = items[i];
            Transform spike = (Transform)spikeData["item"];
            
            if (spike.name != "spike(Clone)") continue;

            float height = float.Parse(spikeData["height"].ToString());

            if (spike.position.y < height)
                spike.position += Vector3.up * Time.deltaTime*2;
        }
    }

    void IceFloor()
    {
        Transform newFloor = Instantiate(iceFloor);
        newFloor.position = transform.position + transform.forward * 15- new Vector3(0, transform.lossyScale.y / 2+0.1f, 0);
        newFloor.rotation = transform.rotation;
        newFloor.tag = "Ice";

        Dictionary<string, object> wallData = new Dictionary<string, object>();
        wallData.Add("item", newFloor);
        wallData.Add("time", 150f);
        items.Add(wallData);
    }

    void CleanUp()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Dictionary<string, object> item = items[i];
            float time = float.Parse(item["time"].ToString());

            if (time > 0)
            {
                float newTime = time - Time.deltaTime;
                item["time"] = newTime.ToString();
            }
            else
            {
                Destroy(((Transform)item["item"]).gameObject);
                items.RemoveAt(i);
            }
        }
    }

    IEnumerator CleanUp(Transform obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj.gameObject);
    }

    IEnumerator CleanUp(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj.gameObject);
    }

    private void Punch()
    {
        Debug.Log("punch!");
        throw new System.NotImplementedException();
    }
}
