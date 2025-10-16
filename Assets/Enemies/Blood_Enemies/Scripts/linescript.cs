using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.HableCurve;
using static UnityEngine.UI.GridLayoutGroup;

public class linescript : MonoBehaviour 
{
    LineRenderer line;
    public float radius;
    public int segments;
    public bool toggleCircle;



    private void Start()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = segments + 1;
        line.startWidth = 1f;
        line.endWidth = 1f;
        line.loop = true;
        line.startColor = Color.red;
        line.endColor = Color.red;

        radius += 1;
            
        toggleCircle = false;
        line.enabled = false;
        
    }

    private void Update()
    {
      
        
        if (!toggleCircle) return;

        if (toggleCircle)
            DrawCircle(gameObject);

        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    public void DrawCircle(GameObject circleCenter)
    {
        line.enabled = true;
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0, z) + circleCenter.transform.position);

            angle += 360f / segments;
        }
    }

    /*
    public void DrawSquare()
    {
        Debug.Log("DRAWING SQUARE");
        line.enabled = true;
        Vector3[] corners = new Vector3[5];

        float cornerPoint = radius;
        //  Each corner apart from corners[4] is the edge of a square. corners[4] connects to first corner to form the square. 
        // corners point is the same value being used in different ways via positive and negative to calculate which corner it should be based off the center of the square.


        corners[0] = new Vector3(-cornerPoint, 0, -cornerPoint); 
        corners[1] = new Vector3(-cornerPoint, 0, cornerPoint); 
        corners[2] = new Vector3(cornerPoint, 0, cornerPoint); 
        corners[3] = new Vector3(cornerPoint, 0, -cornerPoint); 
        corners[4] = corners[0];

        line.positionCount = corners.Length;

        for (int i = 0; i < corners.Length; i++)
            line.SetPosition(i, player.transform.position + corners[i]);

    }

    */
}





