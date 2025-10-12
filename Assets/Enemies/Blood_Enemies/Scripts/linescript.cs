using Unity.VisualScripting;
using UnityEngine;

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
    }

    private void Update()
    {
      
        
        if (!toggleCircle)
        {
            line.enabled = false;
            return;
        }
        line.enabled = true;

        DrawCircle();

        line.startColor = Color.red;
        line.endColor = Color.red;
    }

    private void DrawCircle()
    {
        float angle = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0, z) + transform.position);

            angle += 360f / segments;
        }
    }
}
