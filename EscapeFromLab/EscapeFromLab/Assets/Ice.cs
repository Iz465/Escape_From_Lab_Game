using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ice : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public InputAction action;
    public InputAction wallButton;

    public Transform iceWall;
    
    void Start()
    {
        wallButton.Enable();
        action.Enable();
        action.performed += Punch;
        wallButton.performed += IceWall;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void IceWall(InputAction.CallbackContext context)
    {
        Transform newWall = Instantiate(iceWall);
        newWall.position = transform.position + transform.forward * 5;
        newWall.rotation = transform.rotation;

        StartCoroutine(CleanUp(newWall, 5));
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

    private void Punch(InputAction.CallbackContext obj)
    {
        Debug.Log("punch!");
        throw new System.NotImplementedException();
    }
}
