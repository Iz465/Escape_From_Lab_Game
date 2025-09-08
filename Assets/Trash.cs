using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class Trash : MonoBehaviour
{
    [SerializeField] GameObject tile;
    private void OnEnable()
    {
        for (int x = 0; x < 15; x++)
        {
            for (int z = 0; z < 200; z++)
            {
                GameObject newTile = Instantiate(tile);
                newTile.transform.parent = GameObject.Find("FallingTiles").transform;
                newTile.transform.position = transform.position + transform.right * x;
                newTile.transform.position += transform.forward * z;
            }
        }
    }
}