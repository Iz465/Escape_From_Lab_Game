using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//[ExecuteAlways]
public class Spikes : MonoBehaviour
{
    List<Transform> topArray = new List<Transform>();
    List<Transform> bottomArray = new List<Transform>();

    public float topy;
    public float bottomY;

    bool moveMode = true; // true/false will indicate which direction

    public float moveSpeed = 10;
    void Start()
    {
        int rows = transform.childCount;

        for (int i = 1; i <= rows; i ++)
        {
            Transform row = transform.Find("Row" + i.ToString());
            int spikes = row.childCount;

            for (int j = 0; j < spikes; j++)
            {
                if(i%2==0)
                    topArray.Add(row);
                else
                    bottomArray.Add(row);
            }
        }
    }


    void MoveTopSpikes()
    {
        if (moveMode)
        {
            if (topArray[0].position.y > bottomY)
            {
                topArray[0].position -= new Vector3(0, moveSpeed * Time.deltaTime, 0);
            }
            else
            {
                moveMode = false;
            }
        }
        else
        {
            if(topArray[0].position.y < topy)
            {
                topArray[0].position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
            }
            else
            {
                moveMode = true;
            }
        }
        for(int i = 1; i < topArray.Count; i++)
        {
            topArray[i].position = new Vector3(topArray[i].position.x, topArray[0].position.y, topArray[i].position.z);
        }
    }

    void MoveBottomSpikes()
    {
        for (int i = 0; i < bottomArray.Count; i++)
        {
            Vector3 spikePos = bottomArray[i].position;
            bottomArray[i].position = new Vector3(spikePos.x, topy - topArray[0].position.y, spikePos.z);
            /*if (moveMode)
            {
                //if (bottomArray[i].position.y < bottomY)
                //{
                  //  print("moving 3");
                    bottomArray[i].position -= new Vector3(0, 50 * Time.deltaTime, 0);
                //}
            }
            else
            {
                //if (bottomArray[i].position.y > bottomY)
                //{
                  //  print("moving 4");
                    bottomArray[i].position += new Vector3(0, 50 * Time.deltaTime, 0);
                //}
            }*/
        }
    }

    void Update()
    {
        MoveTopSpikes();
        MoveBottomSpikes();
    }

    
    private void OnEnable()
    {
        
        
    }
    

    // Update is called once per frame
}
