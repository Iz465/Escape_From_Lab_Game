using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void ResetDamage()
    {
        for (int i = 0; i < topArray.Count; i++)
        {
            topArray[i].GetComponent<SpikeDamage>().damaged = false;
            bottomArray[i].GetComponent<SpikeDamage>().damaged = false;
        }
    }

    void MoveTopSpikes()
    {
        float topPosition = 0;
        if (moveMode)
        {
            if (topArray[0].position.y > bottomY)
            {
                topPosition = topArray[0].position.y - moveSpeed * Time.deltaTime;
                topArray[0].Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0), Space.World);
            }
            else
            {
                ResetDamage();
                moveMode = false;
            }
        }
        else
        {
            if(topArray[0].position.y < topy)
            {
                topPosition = topArray[0].position.y + moveSpeed * Time.deltaTime;
                topArray[0].Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0), Space.World);
            }
            else
            {
                ResetDamage();
                moveMode = true;
            }
        }

        for(int i = 1; i < topArray.Count; i++)
        {
            float direction = topPosition - topArray[i].position.y;
            topArray[i].Translate(new Vector3(0,direction*Time.deltaTime, 0), Space.World);
        }
    }

    void MoveBottomSpikes()
    {
        float yGoal = topy - topArray[0].position.y;

        for (int i = 0; i < bottomArray.Count; i++)
        {
            float direction = yGoal - bottomArray[i].position.y;
            bottomArray[i].Translate(new Vector3(0, direction*Time.deltaTime, 0),Space.World);
        }
    }

    void Update()
    {
        MoveTopSpikes();
        MoveBottomSpikes();
    }
}
